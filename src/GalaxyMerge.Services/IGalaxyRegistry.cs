using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public interface IGalaxyRegistry
    {
        bool IsRegistered(string galaxyName, string userName);
        IGalaxyRepository GetGalaxy(string galaxyName, string userName);
        IEnumerable<IGalaxyRepository> GetByName(string galaxyName);
        IEnumerable<IGalaxyRepository> GetByUser(string userName);
        IEnumerable<IGalaxyRepository> GetAll();
        void Register(string galaxyName);
        void Register(string galaxyName, string userName);
        void RegisterAll();
        void RegisterAll(string userName);
        void RegisterParallel();
        Task RegisterAsync(string galaxyName, CancellationToken token);
        Task RegisterAsync(string galaxyName, string userName, CancellationToken token);
        Task RegisterAllAsync(CancellationToken token);
        Task RegisterAllAsync(string userName, CancellationToken token);
        void Unregister(string galaxyName, string userName);
        void Unregister(IEnumerable<string> galaxies, string userName);
    }
}