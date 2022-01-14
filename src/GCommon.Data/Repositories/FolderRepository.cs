using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCommon.Data.Abstractions;
using GCommon.Data.Base;
using GCommon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Data.Repositories
{
    internal class FolderRepository : ReadOnlyRepository<Folder>, IFolderRepository
    {
        internal FolderRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Folder>> GetSymbolHierarchy()
        {
            var results = await Set
                .Include(x => x.Folders)
                .Include(x => x.FolderObjectLinks).ThenInclude(x => x.GalaxyObject)
                .ToListAsync();
            
            return results
                .Where(x => x.Depth == 1 && x.FolderType == 2)
                .OrderBy(x => x.FolderName);
        }
    }
}