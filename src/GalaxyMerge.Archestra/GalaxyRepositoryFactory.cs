using System.Threading;
using System.Threading.Tasks;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Archestra
{
    public class GalaxyRepositoryFactory : IGalaxyRepositoryFactory
    {
        public IGalaxyRepository Create(string galaxyName)
        {
            return new GalaxyRepository(galaxyName);
        }

        public Task<IGalaxyRepository> CreateAsync(string galaxyName, CancellationToken token)
        {
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                return Create(galaxyName);
            }, token);
        }
    }
}