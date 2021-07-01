using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    internal class FolderRepository : Repository<Folder>, IFolderRepository
    {
        internal FolderRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Folder>> GetSymbolHierarchy()
        {
            var results = await Set
                .Include(x => x.Folders)
                .Include(x => x.FolderObjectLinks).ThenInclude(x => x.GObject)
                .ToListAsync();
            
            return results
                .Where(x => x.Depth == 1 && x.FolderType == 2)
                .OrderBy(x => x.FolderName);
        }
    }
}