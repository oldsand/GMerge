using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GalaxyMerge.Archestra.Options;
using GalaxyMerge.Common.Abstractions;

[assembly: InternalsVisibleTo("GalaxyMerge.Archestra.Tests")]

namespace GalaxyMerge.Archestra.Abstractions
{
    public interface IGalaxyRepository
    {
        string Name { get; }
        string Host { get; }
        bool Connected { get; }
        string LoggedInUser { get; }
        string VersionString { get; }
        int? VersionNumber { get; }
        string CdiVersion { get; }
        void SynchronizeClient();
        void Login(string userName);
        Task LoginAsync(string userName, CancellationToken token);
        void Logout();
        bool UserIsAuthorized(string userName);
        IGalaxyObject GetObject(string tagName);
        IEnumerable<IGalaxyObject> GetObjects(IEnumerable<string> tagNames);
        IGalaxySymbol GetSymbol(string tagName);
        IEnumerable<IGalaxySymbol> GetSymbols(IEnumerable<string> tagNames);
        void CreateObject(IGalaxyObject galaxyObject);
        void CreateObjects(IEnumerable<IGalaxyObject> galaxyObjects);
        void CreateSymbol(IGalaxySymbol galaxySymbol);
        void CreateSymbols(IEnumerable<IGalaxySymbol> galaxySymbols);
        void DeleteObject(string tagName, bool recursive);
        void DeleteObjects(IEnumerable<string> tagNames, bool recursive);
        void UpdateObject(IGalaxyObject galaxyObject);
        void Deploy(IEnumerable<string> tagNames, DeploymentOptions options);
        void Undeploy(IEnumerable<string> tagNames, DeploymentOptions options);
        void ExportPkg(string tagName, string fileName);
        void ExportPkg(IEnumerable<string> tagNames, string fileName);
        void ExportCsv(string tagName, string fileName);
        void ExportCsv(IEnumerable<string> tagNames, string fileName);
        void ExportSymbol(string tagName, string destination);
        void ExportSymbol(IEnumerable<string> tagNames, string destination);
        void ImportPkg(string fileName, bool overwrite);
        void ImportCsv(string fileName);
        void ImportSymbol(string fileName, string tagName, bool overwrite);
    }
}