using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCommon.Data.Abstractions;
using GCommon.Data.Base;
using GCommon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Data.Repositories
{
    internal class ObjectRepository : ReadOnlyRepository<GalaxyObject>, IObjectRepository
    {
        internal ObjectRepository(DbContext context) : base (context)
        {
        }

        public GalaxyObject Find(int id)
        {
            return Set.Find(id);
        }

        public IEnumerable<GalaxyObject> Find(string tagName)
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

        public GalaxyObject FindIncludeDescendants(string tagName)
        {
            var results = Set.Include(x => x.Derivations).ToList();
            return results.SingleOrDefault(x => x.TagName == tagName);
        }
        
        public GalaxyObject FindIncludeTemplate(int objectId)
        {
            var results = Set.Include(x => x.TemplateDefinition);
            return results.SingleOrDefault(x => x.ObjectId == objectId);
        }
        
        public GalaxyObject FindIncludeFolder(string tagName)
        {
            return Set
                .Include(x => x.FolderObjectLink)
                .ThenInclude(x => x.Folder)
                .FirstOrDefault(x => x.TagName == tagName);
        }
        
        public async Task<IEnumerable<GalaxyObject>> GetDerivationHierarchy()
        {
            var results = await Set.Include(x => x.Derivations).Include(x => x.TemplateDefinition).ToListAsync();
            return results
                .Where(x => x.DerivedFromId == 0 && !x.IsHidden && x.Derivations.Any() && x.TemplateDefinition.CategoryId != 16)
                .OrderBy(x => x.TagName);
        }
    }
}