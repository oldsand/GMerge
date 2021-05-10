using System.Collections.Generic;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public interface IGalaxyRepositoryProvider
    {
        IEnumerable<IGalaxyRepository> GetAllServiceInstances();
        IGalaxyRepository GetServiceInstance(string galaxyName);
        IGalaxyRepository GetClientInstance(string galaxyName);
    }
}