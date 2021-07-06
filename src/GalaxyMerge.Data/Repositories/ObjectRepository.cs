using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    internal class ObjectRepository : Repository<GObject>, IObjectRepository
    {
        internal ObjectRepository(DbContext context) : base (context)
        {
        }

        public ObjectRepository(string connectionString) : base(GalaxyContext.Create(connectionString))
        {
        }

        public GObject Find(int id)
        {
            return Set.Find(id);
        }

        public IEnumerable<GObject> Find(string tagName)
        {
            return GetQueryable().Where(x => x.TagName == tagName);
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
        
        public GObject FindIncludeTemplate(int objectId)
        {
            var results = Set.Include(x => x.TemplateDefinition);
            return results.SingleOrDefault(x => x.ObjectId == objectId);
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