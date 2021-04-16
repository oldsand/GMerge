using System.Linq;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    public class ObjectRepository : Repository<GObject>, IObjectRepository
    {
        public ObjectRepository(string connectionString) : base(ContextCreator.Create(connectionString))
        {
        }

        public GObject FindById(int objectId)
        {
            return GetQueryable().SingleOrDefault(x => x.ObjectId == objectId);
        }

        public GObject FindByIdIncludeTemplate(int objectId)
        {
            return Set.Include(x => x.Derivations).SingleOrDefault(x => x.ObjectId == objectId);
        }

        public GObject FindByTagName(string tagName)
        {
            return GetQueryable().SingleOrDefault(x => x.TagName == tagName);
        }

        public GObject FindIncludeDerivations(string tagName)
        {
            var results = Set.Include(x => x.Derivations).ToList();
            return results.SingleOrDefault(x => x.TagName == tagName);
        }
    }
}