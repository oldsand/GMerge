using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GServer.Archestra.Abstractions
{
    public interface IGalaxyRepositoryFactory
    {
        IGalaxyRepository Create(string galaxyName);
        IEnumerable<IGalaxyRepository> CreateAll();
    }
}