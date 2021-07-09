using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IFolderReadOnlyRepository : IReadOnlyRepository<Folder>
    {
        Task<IEnumerable<Folder>> GetSymbolHierarchy();
    }
}