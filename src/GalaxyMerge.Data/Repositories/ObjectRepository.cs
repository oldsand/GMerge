using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    public class ObjectRepository : Repository<GObject>, IObjectRepository
    {
        public ObjectRepository(string galaxyName) 
            : base(ContextCreator.Create(ConnectionStringBuilder.BuildGalaxyConnection(galaxyName)))
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
    }
}