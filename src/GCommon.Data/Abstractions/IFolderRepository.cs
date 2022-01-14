using System.Collections.Generic;
using System.Threading.Tasks;
using GCommon.Data.Base;
using GCommon.Data.Entities;

namespace GCommon.Data.Abstractions
{
    public interface IFolderRepository : IReadOnlyRepository<Folder>
    {
        Task<IEnumerable<Folder>> GetSymbolHierarchy();
    }
}