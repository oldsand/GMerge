using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GCommon.Primitives;
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
        void Login(string userName);
        void Logout();
        bool UserIsAuthorized(string userName);
        ArchestraObject GetObject(string tagName);
        ArchestraGraphic GetGraphic(string tagName);
        void CreateObject(ArchestraObject source);
        void CreateGraphic(ArchestraGraphic archestraGraphic);
        void DeleteObject(string tagName, bool recursive);
        void DeleteObjects(IEnumerable<string> tagNames, bool recursive);
        void UpdateObject(ArchestraObject archestraObject);
        void UpdateGraphic(ArchestraGraphic archestraGraphic);
        void Deploy(IEnumerable<string> tagNames, DeploymentOptions options);
        void Undeploy(IEnumerable<string> tagNames, DeploymentOptions options);
        
        
    }
}