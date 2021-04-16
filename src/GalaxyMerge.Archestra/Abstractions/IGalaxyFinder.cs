using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GalaxyMerge.Archestra.Abstractions
{
    public interface IGalaxyFinder
    {
        bool Exists(string galaxyName);
        Task<bool> ExistsAsync(string galaxyName, CancellationToken token);
        IEnumerable<string> FindAll();
        Task<IEnumerable<string>> FindAllAsync(CancellationToken token);
    }
}