using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using ArchestrA.Visualization.GraphicAccess;
using GalaxyMerge.Archestra.Extensions;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archestra.Options;
using GalaxyMerge.Core.Utilities;

namespace GalaxyMerge.Archestra
{
    public class GalaxyRepository : IGalaxyRepository
    {
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
                    $"Unable to load galaxy {galaxyName} on {Environment.MachineName}. Failed on {result.ID}. {result.CustomMessage}",
                    new GalaxyCommandResult(result));
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
            ResultHandler.Handle(_galaxy.CommandResult, _galaxy.Name);
        }

        public void Login(string userName)
        {
            _galaxy.Login(userName, string.Empty);
            ResultHandler.Handle(_galaxy.CommandResult, _galaxy.Name);
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
            _galaxy.Logout();
            ResultHandler.Handle(_galaxy.CommandResult, _galaxy.Name);
            Connected = false;
            ConnectedUser = string.Empty;
        }

        public bool UserIsAuthorized(string userName)
        {
            var security = _galaxy.GetReadOnlySecurity();
            
            foreach (IGalaxyUser user in security.UsersAvailable)
                if (user.UserName == userName)
                    return true;
            
            return false;
        }
        
        public GalaxyObject GetObject(string tagName)
        {
            _galaxy.SynchronizeClient();
            var gObject = _galaxy.GetObjectByName(tagName);
            return gObject?.AsGalaxyObject();
        }

        public IEnumerable<GalaxyObject> GetObjects(IEnumerable<string> tagNames)
        {
            _galaxy.SynchronizeClient();
            var objects = _galaxy.GetObjectsByName(tagNames);
            foreach (IgObject gObject in objects)
                yield return gObject.AsGalaxyObject();
        }

        public GalaxySymbol GetSymbol(string tagName)
        {
            _galaxy.SynchronizeClient();
            using var tempDirectory = new TempDirectory(ApplicationPath.TempSymbol);
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
            var symbol = galaxySymbol.ToXml();
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), symbol);
            SchemaValidator.ValidateSymbol(doc);

            using var tempDirectory = new TempDirectory(ApplicationPath.TempSymbol);
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
            
            ResultHandler.Handle(objects.CommandResults, _galaxy.Name);
        }

        public void UpdateObject(GalaxyObject galaxyObject)
        {
            var repositoryObject = _galaxy.GetObjectByName(galaxyObject.TagName);
            var original = repositoryObject.AsGalaxyObject();
            
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
            ResultHandler.Handle(_galaxy.CommandResults, _galaxy.Name);
        }

        public void Undeploy(IEnumerable<string> tagNames, DeploymentOptions options = null)
        {
            var instances = _galaxy.GetInstancesByName(tagNames);
            instances.Undeploy();
            ResultHandler.Handle(_galaxy.CommandResults, _galaxy.Name);
        }

        public void ExportPkg(string tagName, string fileName)
        {
            var collection = _galaxy.CreategObjectCollection();
            var item = _galaxy.GetObjectByName(tagName);
            collection.Add(item);

            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            ResultHandler.Handle(collection.CommandResults, _galaxy.Name);
        }

        public void ExportPkg(IEnumerable<string> tagNames, string fileName)
        {
            var collection = _galaxy.GetObjectsByName(tagNames);
            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            ResultHandler.Handle(collection.CommandResults, _galaxy.Name);
        }

        public void ExportCsv(string tagName, string fileName)
        {
            var collection = _galaxy.CreategObjectCollection();
            var item = _galaxy.GetObjectByName(tagName);
            collection.Add(item);

            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            ResultHandler.Handle(collection.CommandResults, _galaxy.Name);
        }

        public void ExportCsv(IEnumerable<string> tagNames, string fileName)
        {
            var collection = _galaxy.GetObjectsByName(tagNames);
            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            ResultHandler.Handle(collection.CommandResults, _galaxy.Name);
        }

        public void ExportSymbol(string tagName, string fileName)
        {
            var result = _graphicAccess.ExportGraphicToXml(_galaxy, tagName, fileName);
            ResultHandler.Handle(result, tagName);
        }

        public void ExportSymbol(IEnumerable<string> tagNames, string destination)
        {
            foreach (var tagName in tagNames)
            {
                var fileName = Path.Combine(destination, $"{tagName}.xml");
                var result = _graphicAccess.ExportGraphicToXml(_galaxy, tagName, fileName);
                ResultHandler.Handle(result, tagName);
            }
        }

        public void ImportPkg(string fileName, bool overwrite)
        {
            _galaxy.ImportObjects(fileName, overwrite);
            ResultHandler.Handle(_galaxy.CommandResults, _galaxy.Name);
        }

        public void ImportCsv(string fileName)
        {
            _galaxy.GRLoad(fileName, GRLoadMode.GRLoadModeUpdate);
            ResultHandler.Handle(_galaxy.CommandResults, _galaxy.Name);
        }

        public void ImportSymbol(string fileName, string tagName, bool overwrite)
        {
            var result = _graphicAccess.ImportGraphicFromXml(_galaxy, tagName, fileName, overwrite);
            ResultHandler.Handle(result, tagName);
        }

        internal IGalaxy GetGalaxy()
        {
            return _galaxy;
        }
    }
}