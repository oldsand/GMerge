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
    public class ObjectRepository : Repository<GObject>, IObjectRepository
    {
        public ObjectRepository(string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(galaxyName)))
        {
        }
        
        public ObjectRepository(string hostName, string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(hostName, galaxyName)))
        {
        }
        
        public ObjectRepository(DbConnectionStringBuilder connectionStringBuilder) 
            : base(GalaxyContext.Create(connectionStringBuilder.ConnectionString))
        {
        }

        public GObject FindByTagName(string tagName)
        {
            return GetQueryable().FirstOrDefault(x => x.TagName == tagName);
        }

        public string GetTagName(int objectId)
        {
            return Set.Find(objectId).TagName;
        }

        public int GetObjectId(string tagName)
        {
            var gObject = Set.FirstOrDefault(x => x.TagName == tagName);
            return gObject?.ObjectId ?? 0;
        }

        public GObject FindIncludeDescendants(string tagName)
        {
            var results = Set.Include(x => x.Derivations).ToList();
            return results.SingleOrDefault(x => x.TagName == tagName);
        }
        
        public GObject FindIncludeFolder(string tagName)
        {
            return Set
                .Include(x => x.FolderObjectLink)
                .ThenInclude(x => x.Folder)
                .FirstOrDefault(x => x.TagName == tagName);
        }
        
        public async Task<IEnumerable<GObject>> GetDerivationHierarchy()
        {
            var results = await Set.Include(x => x.Derivations).Include(x => x.TemplateDefinition).ToListAsync();
            return results
                .Where(x => x.DerivedFromId == 0 && !x.IsHidden && x.Derivations.Any() && x.TemplateDefinition.CategoryId != 16)
                .OrderBy(x => x.TagName);
        }
        
    }
}