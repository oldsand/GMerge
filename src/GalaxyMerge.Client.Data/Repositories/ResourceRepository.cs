using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Client.Data.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppContext _context;

        public ResourceRepository()
        {
            _context = new AppContext();
        }
        
        public ResourceRepository(string dataSource)
        {
            var options = new DbContextOptionsBuilder<AppContext>().UseSqlite(dataSource).Options;
            _context = new AppContext(options);
        }
        
        public ResourceRepository(DbConnection connection)
        {
            var options = new DbContextOptionsBuilder<AppContext>().UseSqlite(connection).Options;
            _context = new AppContext(options);
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        public ResourceEntry Get(int id)
        {
            return _context.Set<ResourceEntry>().Find(id);
        }

        public ResourceEntry Get(string name)
        {
            return _context.Set<ResourceEntry>().SingleOrDefault(x => x.ResourceName == name);
        }

        public IEnumerable<ResourceEntry> GetAll()
        {
            return _context.Set<ResourceEntry>().ToList();
        }

        public IEnumerable<string> GetNames()
        {
            return _context.Set<ResourceEntry>().Select(x => x.ResourceName).ToList();
        }

        public async Task<IEnumerable<string>> GetNamesAsync()
        {
            return await _context.Set<ResourceEntry>().Select(x => x.ResourceName).ToListAsync();
        }

        public async Task<IEnumerable<ResourceEntry>> GetAllAsync()
        {
            return await _context.Set<ResourceEntry>()
                .Include(x => x.Connection)
                .Include(x => x.Archive)
                .Include(x => x.Directory)
                .ToListAsync();
        }

        public void Add(ResourceEntry resourceEntry)
        {
            _context.Set<ResourceEntry>().Add(resourceEntry);
        }

        public void Remove(ResourceEntry resourceEntry)
        {
            _context.Set<ResourceEntry>().Remove(resourceEntry);
        }

        public void Update(ResourceEntry resourceEntry)
        {
            _context.Set<ResourceEntry>().Update(resourceEntry);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}