using System.Collections.Generic;
using System.Linq;
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
        
        public void Dispose()
        {
            _context.Dispose();
        }

        public GalaxyResource Get(int id)
        {
            return _context.Set<GalaxyResource>().Find(id);
        }

        public GalaxyResource Get(string name)
        {
            return _context.Set<GalaxyResource>().FirstOrDefault(x => x.ResourceName == name);
        }

        public IEnumerable<GalaxyResource> GetAll()
        {
            return _context.Set<GalaxyResource>().ToList();
        }

        public void Add(GalaxyResource resource)
        {
            _context.Set<GalaxyResource>().Add(resource);
        }

        public void Remove(GalaxyResource resource)
        {
            _context.Set<GalaxyResource>().Remove(resource);
        }

        public void Update(GalaxyResource resource)
        {
            _context.Set<GalaxyResource>().Update(resource);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}