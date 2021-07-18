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
        ArchestraObject GetObject(string tagName);
        IEnumerable<ArchestraObject> GetObjects(IEnumerable<string> tagNames);
        ArchestraGraphic GetGraphic(string tagName);
        IEnumerable<ArchestraGraphic> GetGraphics(IEnumerable<string> tagNames);
        void CreateObject(ArchestraObject archestraObject);
        void CreateObjects(IEnumerable<ArchestraObject> galaxyObjects);
        void CreateGraphic(ArchestraGraphic archestraGraphic);
        void CreateSymbols(IEnumerable<ArchestraGraphic> galaxySymbols);
        void DeleteObject(string tagName, bool recursive);
        void DeleteObjects(IEnumerable<string> tagNames, bool recursive);
        void UpdateObject(ArchestraObject archestraObject);
        void UpdateSymbol(ArchestraGraphic archestraGraphic);
        void Deploy(IEnumerable<string> tagNames, DeploymentOptions options);
        void Undeploy(IEnumerable<string> tagNames, DeploymentOptions options);
        
        
    }
}