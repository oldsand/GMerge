using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private readonly GalaxyContext _context;

        public LookupRepository(string galaxyName)
        {
            _context = GalaxyContext.Create(ConnectionStringBuilder.BuildGalaxyConnection(galaxyName));
        }
        
        public IEnumerable<ObjectLookup> FindAncestors(int objectId, bool excludeSelf = true)
        {
            return _context.ObjectLookups
                .FromSqlInterpolated($"exec dbo.internal_list_ancestors_for_object {objectId}, {excludeSelf}")
                .ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}