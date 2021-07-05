using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using ArchestrA.Visualization.GraphicAccess;
using GalaxyMerge.Archestra.Extensions;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archestra.Exceptions;
using GalaxyMerge.Archestra.Helpers;
using GalaxyMerge.Archestra.Options;
using GalaxyMerge.Core.Utilities;
using NLog;

namespace GalaxyMerge.Archestra
{
    public class GalaxyRepository : IGalaxyRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable because this has to be in memory for operations to work (according to documentation)
        private readonly GRAccessAppClass _grAccessApp;
        private readonly GraphicAccess _graphicAccess;
        private readonly IGalaxy _galaxy;

        public GalaxyRepository(string galaxyName)
        {
            _grAccessApp = new GRAccessAppClass();
            _graphicAccess = new GraphicAccess();
            _galaxy = _grAccessApp.QueryGalaxies(Environment.MachineName)[galaxyName];
            
            var result = _grAccessApp.CommandResult;
            if (!result.Successful || _galaxy == null)
                throw new GalaxyException(
                    $"Unable to load galaxy {galaxyName} on {Environment.MachineName}. Failed on {result.ID}. {result.CustomMessage}");
        }

        internal GalaxyRepository(GRAccessAppClass grAccessApp, IGalaxy galaxy)
        {
            _grAccessApp = grAccessApp ?? throw new ArgumentNullException(nameof(grAccessApp), "Value cannot be null");
            _galaxy = galaxy ?? throw new ArgumentNullException(nameof(grAccessApp), "Value cannot be null");
            _graphicAccess = new GraphicAccess();
        }

        public string Name => _galaxy.Name;
        public string Host => Environment.MachineName;
        public bool Connected { get; private set; }
        public string ConnectedUser { get; private set; }
        public string VersionString => _galaxy?.VersionString;
        public int? VersionNumber => _galaxy?.VersionNumber;
        public string CdiVersion => _galaxy?.CdiVersionString;

        public void SynchronizeClient()
        {
            _galaxy.SynchronizeClient();
            _galaxy.CommandResult.Process();
        }

        public void Login(string userName)
        {
            Logger.Trace("Logging into galaxy {Galaxy} with user name {User}", Name, userName);
            
            _galaxy.SecureLogin(userName);

            Connected = true;
            ConnectedUser = userName;
        }

        public Task LoginAsync(string userName, CancellationToken token)
        {
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                Login(userName);
            }, token);
        }

        public void Logout()
        {
            Logger.Trace("User {User} logging out of galaxy {Galaxy}", ConnectedUser, Name);
            
            _galaxy.Logout();
            _galaxy.CommandResult.Process();
            
            Connected = false;
            ConnectedUser = string.Empty;
        }

        public bool UserIsAuthorized(string userName)
        {
            Logger.Trace("Authorizing {User} against {Galaxy} current security settings", ConnectedUser, Name);
            
            _galaxy.SynchronizeClient();
            var security = _galaxy.GetReadOnlySecurity();
            _galaxy.CommandResult.Process();
            
            foreach (IGalaxyUser user in security.UsersAvailable)
                if (user.UserName == userName)
                    return true;
            
            return false;
        }

        public GalaxyObject GetObject(string tagName)
        {
            _galaxy.SynchronizeClient();
            var gObject = _galaxy.GetObjectByName(tagName);
            return gObject?.Map();
        }

        public IEnumerable<GalaxyObject> GetObjects(IEnumerable<string> tagNames)
        {
            _galaxy.SynchronizeClient();
            var objects = _galaxy.GetObjectsByName(tagNames);
            foreach (IgObject gObject in objects)
                yield return gObject.Map();
        }

        public GalaxySymbol GetSymbol(string tagName)
        {
            _galaxy.SynchronizeClient();
            using var tempDirectory = new TempDirectory(ApplicationPath.TempSymbolSubPath);
            var fileName = Path.Combine(tempDirectory.FullName, $@"{tagName}.xml");
            
            ExportSymbol(tagName, fileName);
            var symbol = XElement.Load(fileName);
            
            return new GalaxySymbol(tagName).FromXml(symbol);
        }

        public IEnumerable<GalaxySymbol> GetSymbols(IEnumerable<string> tagNames)
        {
            return tagNames.Select(GetSymbol);
        }

        public void CreateObject(GalaxyObject galaxyObject)
        {
            _galaxy.SynchronizeClient();
            var repositoryObject = _galaxy.CreateObject(galaxyObject.TagName, galaxyObject.DerivedFromName);
            
            try
            {
                repositoryObject.CheckOut();
                
                repositoryObject.SetUserDefinedAttributes(galaxyObject);
                repositoryObject.SetFieldAttributes(galaxyObject);
                repositoryObject.Save();
                
                repositoryObject.ConfigureAttributes(galaxyObject);
                repositoryObject.Save();
                
                repositoryObject.ConfigureExtensions(galaxyObject);
                repositoryObject.Save();
                
                repositoryObject.CheckIn($"Galaxy Merge Service Created Object '{galaxyObject.TagName}'");
            }
            catch (Exception)
            {
                repositoryObject.CheckIn();
                repositoryObject.Delete();
                throw;
            }
        }

        public void CreateObjects(IEnumerable<GalaxyObject> galaxyObjects)
        {
            foreach (var galaxyObject in galaxyObjects)
                CreateObject(galaxyObject);
        }

        public void CreateSymbol(GalaxySymbol galaxySymbol)
        {
            _galaxy.SynchronizeClient();
            
            var symbol = galaxySymbol.ToXml();
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), symbol);
            SchemaValidator.ValidateSymbol(doc);

            using var tempDirectory = new TempDirectory(ApplicationPath.TempSymbolSubPath);
            var fileName = Path.Combine(tempDirectory.FullName, $"{galaxySymbol.TagName}.xml");
            doc.Save(fileName);
            
            ImportSymbol(fileName, galaxySymbol.TagName, false);
            
            //TODO: Can we then set the folder container or no?
        }

        public void CreateSymbols(IEnumerable<GalaxySymbol> galaxySymbols)
        {
            foreach (var symbol in galaxySymbols)
                CreateSymbol(symbol);
        }

        public void DeleteObject(string tagName, bool recursive)
        {
            _galaxy.SynchronizeClient();
            
            if (recursive)
            {
                _galaxy.DeepDelete(tagName);
                return;
            }
            
            var gObject = _galaxy.GetObjectByName(tagName);
            gObject?.Delete();
        }

        public void DeleteObjects(IEnumerable<string> tagNames, bool recursive)
        {
            var objects = _galaxy.GetTemplatesByName(tagNames);
            
            foreach (IgObject gObject in objects)
                DeleteObject(gObject.Tagname, recursive);
            
            objects.CommandResults.Process();
        }

        public void UpdateObject(GalaxyObject galaxyObject)
        {
            _galaxy.SynchronizeClient();
            
            var repositoryObject = _galaxy.GetObjectByName(galaxyObject.TagName);
            var original = repositoryObject.Map();
            
            try
            {
                repositoryObject.CheckOut();
                
                repositoryObject.SetUserDefinedAttributes(galaxyObject);
                repositoryObject.SetFieldAttributes(galaxyObject);
                repositoryObject.Save();
                
                repositoryObject.ConfigureAttributes(galaxyObject);
                repositoryObject.Save();
                
                repositoryObject.ConfigureExtensions(galaxyObject);
                repositoryObject.Save();
                
                repositoryObject.CheckIn($"Galaxy Merge Service Created Object '{galaxyObject.TagName}'");
            }
            catch (Exception)
            {
                repositoryObject.CheckIn();
                repositoryObject.Delete();
                CreateObject(original);
                throw;
            }
        }

        public void UpdateSymbol(GalaxySymbol galaxySymbol)
        {
            throw new NotImplementedException();
        }

        public void Deploy(IEnumerable<string> tagNames, DeploymentOptions options)
        {
            if (options == null) throw new ArgumentException("Value cannot be null");
            
            _galaxy.SynchronizeClient();

            var deployedOption = options.DeployedOption.ToMxType();
            var skipUnDeployed = options.SkipUnDeployed
                ? ESkipIfCurrentlyUndeployed.doSkipIfCurrentlyUndeployed
                : ESkipIfCurrentlyUndeployed.dontSkipIfCurrentlyUndeployed;
            var deployOnScan = options.DeployOnScan
                ? EDeployOnScan.doDeployOnScan
                : EDeployOnScan.dontDeployOnScan;
            var forceOffScan = options.ForceOffScan
                ? EForceOffScan.doForceOffScan
                : EForceOffScan.dontForceOffScan;

            var instances = _galaxy.GetInstancesByName(tagNames);

            instances.Deploy(deployedOption, skipUnDeployed, deployOnScan, forceOffScan,
                options.MarkAsDeployOnStatusMismatch);
            _galaxy.CommandResults.Process();
        }

        public void Undeploy(IEnumerable<string> tagNames, DeploymentOptions options = null)
        {
            var instances = _galaxy.GetInstancesByName(tagNames);
            instances.Undeploy();
            _galaxy.CommandResults.Process();
        }

        public void ExportPkg(string tagName, string fileName)
        {
            _galaxy.SynchronizeClient();
            
            var collection = _galaxy.CreategObjectCollection();
            var item = _galaxy.GetObjectByName(tagName);
            collection.Add(item);

            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            collection.CommandResults.Process();
        }

        public void ExportPkg(IEnumerable<string> tagNames, string fileName)
        {
            _galaxy.SynchronizeClient();
            
            var collection = _galaxy.GetObjectsByName(tagNames);
            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            collection.CommandResults.Process();
        }

        public void ExportCsv(string tagName, string fileName)
        {
            _galaxy.SynchronizeClient();
            
            var collection = _galaxy.CreategObjectCollection();
            var item = _galaxy.GetObjectByName(tagName);
            collection.Add(item);

            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            collection.CommandResults.Process();
        }

        public void ExportCsv(IEnumerable<string> tagNames, string fileName)
        {
            _galaxy.SynchronizeClient();
            
            var collection = _galaxy.GetObjectsByName(tagNames);
            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            collection.CommandResults.Process();
        }

        public void ExportSymbol(string tagName, string fileName)
        {
            _galaxy.SynchronizeClient();
            
            var result = _graphicAccess.ExportGraphicToXml(_galaxy, tagName, fileName);
            result.Process();
        }

        public void ExportSymbol(IEnumerable<string> tagNames, string destination)
        {
            _galaxy.SynchronizeClient();
            
            foreach (var tagName in tagNames)
            {
                var fileName = Path.Combine(destination, $"{tagName}.xml");
                var result = _graphicAccess.ExportGraphicToXml(_galaxy, tagName, fileName);
                result.Process();
            }
        }

        public void ImportPkg(string fileName, bool overwrite)
        {
            _galaxy.SynchronizeClient();
            
            _galaxy.ImportObjects(fileName, overwrite);
            _galaxy.CommandResults.Process();
        }

        public void ImportCsv(string fileName)
        {
            _galaxy.SynchronizeClient();
            
            _galaxy.GRLoad(fileName, GRLoadMode.GRLoadModeUpdate);
            _galaxy.CommandResults.Process();
        }

        public void ImportSymbol(string fileName, string tagName, bool overwrite)
        {
            _galaxy.SynchronizeClient();
            
            var result = _graphicAccess.ImportGraphicFromXml(_galaxy, tagName, fileName, overwrite);
            result.Process();
        }

        internal IGalaxy GetGalaxy()
        {
            return _galaxy;
        }
    }
}