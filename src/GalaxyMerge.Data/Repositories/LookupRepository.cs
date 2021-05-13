using System.Collections.Generic;
using System.Data;
using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.Data.SqlClient;
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
        
        public IEnumerable<ObjectLookup> FindDescendants(int objectId, bool includeInstances = false)
        {
            return _context.ObjectLookups
                .FromSqlInterpolated($"exec dbo.internal_list_descendents_for_object {objectId}, {!includeInstances}")
                .ToList();
        }

        public string GetFolderPath(int objectId, int folderType = 2)
        {
            var sql = $"exec @result= dbo.get_folder_path {objectId}, {folderType}";
            var result = new SqlParameter("@result", SqlDbType.NVarChar, 650) {Direction = ParameterDirection.Output};

            _context.Database.ExecuteSqlRaw(sql, result);

            return result.Value.ToString().Replace('$', '\\');
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}