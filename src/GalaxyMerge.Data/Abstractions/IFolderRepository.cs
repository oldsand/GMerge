using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IFolderRepository : IRepository<Folder>
    {
        Task<IEnumerable<Folder>> GetSymbolHierarchy();
    }
}