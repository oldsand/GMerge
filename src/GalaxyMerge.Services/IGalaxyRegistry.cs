using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    /// <summary>
    /// Provides service for initializing and storing references to logged in galaxy connections. This is to expedite the
    /// process of retrieving or updating data when a user makes a service request, since the process of loading and
    /// connecting a galaxy is not a fast operation.
    /// </summary>
    public interface IGalaxyRegistry
    {
        bool IsRegistered(string galaxyName, string userName);
        IGalaxyRepository GetGalaxy(string galaxyName, string userName);
        IEnumerable<IGalaxyRepository> GetByGalaxy(string galaxyName);
        IEnumerable<IGalaxyRepository> GetByUser(string userName);
        IEnumerable<IGalaxyRepository> GetAll();
        void Register(string galaxyName);
        void Register(string galaxyName, string userName);
        Task RegisterAsync(string galaxyName, CancellationToken token);
        Task RegisterAsync(string galaxyName, string userName, CancellationToken token);
        Task RegisterAllAsync(string userName, CancellationToken token);
        void Unregister(string galaxyName, string userName);
        void Unregister(IEnumerable<string> galaxies, string userName);
    }
}