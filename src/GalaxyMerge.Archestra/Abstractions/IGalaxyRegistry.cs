using System.Collections.Generic;

namespace GalaxyMerge.Archestra.Abstractions
{
    /// <summary>
    /// Provides service for initializing and storing references to logged in galaxy connections. This is to expedite the
    /// process of retrieving or updating data when a user makes a service request, since the process of loading and
    /// connecting a galaxy is not a fast operation.
    /// </summary>
    public interface IGalaxyRegistry
    {
        bool IsGalaxyRegistered(string galaxyName, string userName);
        IGalaxyRepository GetGalaxy(string galaxyName, string userName);
        IEnumerable<IGalaxyRepository> GetUserGalaxies(string userName);
        IEnumerable<IGalaxyRepository> GetAllGalaxies();
        void RegisterGalaxy(string galaxyName, string userName);
        void UnregisterGalaxy(string galaxyName, string userName);
        
    }
}