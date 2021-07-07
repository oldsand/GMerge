using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archiving.Repositories
{
    internal class ArchiveObjectRepository : IArchiveObjectRepository
    {
        private readonly ArchiveContext _context;

        public ArchiveObjectRepository(ArchiveContext context)
        {
            _context = context;
        }
        
        public bool Exists(int objectId)
        {
            return _context.Objects.Any(x => x.ObjectId == objectId);
        }

        public ArchiveObject Get(int objectId)
        {
            return _context.Objects.Include(x => x.Entries).Single(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveObject> Get(string tagName)
        {
            return _context.Objects.Where(x => x.TagName == tagName);
        }

        public IEnumerable<ArchiveObject> GetAll()
        {
            return _context.Objects.ToList();
        }

        public void Add(ArchiveObject archiveObject)
        {
            _context.Objects.Add(archiveObject);
        }

        public void Remove(int objectId)
        {
            var target = _context.Objects.Find(objectId);
            _context.Objects.Remove(target);
        }

        public void Update(ArchiveObject archiveObject)
        {
            _context.Objects.Update(archiveObject);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}