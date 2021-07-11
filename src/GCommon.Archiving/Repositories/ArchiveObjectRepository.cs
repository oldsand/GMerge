using System.Collections.Generic;
using System.Linq;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving.Repositories
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
            return _context.Objects.Include(x => x.Entries).SingleOrDefault(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveObject> GetAll()
        {
            return _context.Objects.ToList();
        }
        
        public IEnumerable<ArchiveObject> FindByTagName(string tagName)
        {
            return _context.Objects.Where(x => x.TagName == tagName);
        }

        public void Upsert(ArchiveObject archiveObject)
        {
            if (_context.Objects.All(x => x.ObjectId != archiveObject.ObjectId))
            {
                _context.Entry(archiveObject).State = EntityState.Added;
                _context.Objects.Add(archiveObject);
                return;
            }

            _context.Objects.Update(archiveObject);
            /*var target = _context.Objects.Single(x => x.ObjectId == archiveObject.ObjectId);
            target.UpdateTagName(archiveObject.TagName);
            target.UpdateVersion(archiveObject.Version);
            target.AddEntries(archiveObject.Entries);*/
        }
        
        public void Delete(int objectId)
        {
            var target = _context.Objects.Find(objectId);
            if (target == null) return;
            _context.Objects.Remove(target);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}