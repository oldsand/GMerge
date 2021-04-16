using System.Threading;
using System.Threading.Tasks;

namespace GalaxyMerge.Archestra.Abstractions
{
    public interface IGalaxyFactory
    {
        IGalaxyRepository Create(string galaxyName);
        Task<IGalaxyRepository> CreateAsync(string galaxyName, CancellationToken token);
    }
}