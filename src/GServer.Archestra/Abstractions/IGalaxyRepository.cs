using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GServer.Archestra.Entities;
using GServer.Archestra.Options;

namespace GServer.Archestra.Abstractions
{
    public interface IGalaxyRepository
    {
        string Name { get; }
        string Host { get; }
        bool Connected { get; }
        string ConnectedUser { get; }
        string VersionString { get; }
        int? VersionNumber { get; }
        string CdiVersion { get; }
        void SynchronizeClient();
        void Login(string userName);
        Task LoginAsync(string userName, CancellationToken token);
        void Logout();
        bool UserIsAuthorized(string userName);
        GalaxyObject GetObject(string tagName);
        IEnumerable<GalaxyObject> GetObjects(IEnumerable<string> tagNames);
        GalaxySymbol GetSymbol(string tagName);
        IEnumerable<GalaxySymbol> GetSymbols(IEnumerable<string> tagNames);
        void CreateObject(GalaxyObject galaxyObject);
        void CreateObjects(IEnumerable<GalaxyObject> galaxyObjects);
        void CreateSymbol(GalaxySymbol galaxySymbol);
        void CreateSymbols(IEnumerable<GalaxySymbol> galaxySymbols);
        void DeleteObject(string tagName, bool recursive);
        void DeleteObjects(IEnumerable<string> tagNames, bool recursive);
        void UpdateObject(GalaxyObject galaxyObject);
        void UpdateSymbol(GalaxySymbol galaxySymbol);
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