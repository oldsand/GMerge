using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GalaxyMerge.Archestra.Abstractions
{
    public interface IGalaxyRepositoryFactory
    {
        IGalaxyRepository Create(string galaxyName);
        Task<IGalaxyRepository> CreateAsync(string galaxyName, CancellationToken token);
        IEnumerable<IGalaxyRepository> CreateAll();
    }
}