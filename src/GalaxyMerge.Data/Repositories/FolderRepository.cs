using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(galaxyName)))
        {
        }
        
        public FolderRepository(string hostName, string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(hostName, galaxyName)))
        {
        }
        
        public FolderRepository(DbConnectionStringBuilder connectionStringBuilder) 
            : base(GalaxyContext.Create(connectionStringBuilder.ConnectionString))
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