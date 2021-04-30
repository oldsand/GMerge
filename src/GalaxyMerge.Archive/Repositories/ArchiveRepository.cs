using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive.Repositories
{
    public class ArchiveRepository : IArchiveRepository
    {
        private readonly ArchiveContext _context;
        
        public ArchiveRepository(string galaxyName)
        {
            var connectionString = ConnectionStringBuilder.BuildArchiveConnection(galaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
        }
        
        public ArchiveInfo GetInfo()
        {
            return _context.Info.SingleOrDefault();
        }
        
        public bool ObjectExists(int objectId)
        {
            return _context.Objects.Any(x => x.ObjectId == objectId);
        }

        public ArchiveObject GetObject(int objectId)
        {
            return _context.Objects.Find(objectId);
        }
        
        public ArchiveObject GetObjectIncludeEntries(int objectId)
        {
            return _context.Objects.Include(x => x.Entries).SingleOrDefault(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveObject> FindObjectsByTagName(string tagName)
        {
            return _context.Objects.Where(x => x.TagName == tagName);
        }

        public bool HasEntries(int objectId)
        {
            return _context.Entries.Any(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveEntry> FindEntriesByObjectId(int objectId)
        {
            return _context.Entries.Where(x => x.ObjectId == objectId);
        }

        public ArchiveEntry GetLatestEntry(int objectId)
        {
            return _context.Entries.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.ObjectId == objectId);
        }
        
        public bool HasEvent(string eventName)
        {
            return _context.Events.Any(x => x.EventName == eventName);
        }

        public ArchiveEvent GetEvent(int eventId)
        {
            return _context.Events.Find(eventId);
        }

        public IEnumerable<ArchiveEvent> GetEvents()
        {
            return _context.Events.ToList();
        }
        
        public bool HasExclusion(string exclusionName)
        {
            return _context.Exclusions.Any(x => x.ExclusionName == exclusionName);
        }

        public ArchiveExclusion GetExclusion(int exclusionId)
        {
            return _context.Exclusions.Find(exclusionId);
        }

        public IEnumerable<ArchiveExclusion> GetExclusions()
        {
            return _context.Exclusions.ToList();
        }

        public void UpdateInfo(ArchiveInfo archiveInfo)
        {
            _context.Info.Update(archiveInfo);
        }

        public void AddObject(ArchiveObject archiveObject)
        {
            _context.Objects.Add(archiveObject);
        }

        public void RemoveObject(ArchiveObject archiveObject)
        {
            _context.Objects.Remove(archiveObject);
        }

        public void UpdateObject(ArchiveObject archiveObject)
        {
            _context.Objects.Update(archiveObject);
        }

        public void AddEntry(ArchiveEntry archiveEntry)
        {
            _context.Entries.Add(archiveEntry);
        }

        public void AddEvent(ArchiveEvent archiveEvent)
        {
            _context.Events.Add(archiveEvent);
        }

        public void RemoveEvent(ArchiveEvent archiveEvent)
        {
            _context.Events.Remove(archiveEvent);
        }

        public void AddExclusion(ArchiveExclusion archiveExclusion)
        {
            _context.Exclusions.Remove(archiveExclusion);
        }

        public void RemoveExclusion(ArchiveExclusion archiveExclusion)
        {
            _context.Exclusions.Remove(archiveExclusion);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}