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
        
        public GalaxyInfo GetInfo()
        {
            return _context.GalaxyInfo.SingleOrDefault();
        }
        
        public bool ObjectExists(int objectId)
        {
            return _context.ArchiveObjects.Any(x => x.ObjectId == objectId);
        }

        public ArchiveObject GetObject(int objectId)
        {
            return _context.ArchiveObjects.Find(objectId);
        }
        
        public ArchiveObject GetObjectIncludeEntries(int objectId)
        {
            return _context.ArchiveObjects.Include(x => x.Entries).SingleOrDefault(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveObject> FindObjectsByTagName(string tagName)
        {
            return _context.ArchiveObjects.Where(x => x.TagName == tagName);
        }

        public bool HasEntries(int objectId)
        {
            return _context.ArchiveEntries.Any(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveEntry> FindEntriesByObjectId(int objectId)
        {
            return _context.ArchiveEntries.Where(x => x.ObjectId == objectId);
        }

        public ArchiveEntry GetLatestEntry(int objectId)
        {
            return _context.ArchiveEntries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault(x => x.ObjectId == objectId);
        }
        
        public EventSetting GetEventSetting(int eventId)
        {
            return _context.OperationSettings.Find(eventId);
        }

        public IEnumerable<EventSetting> GetEventSettings()
        {
            return _context.OperationSettings.ToList();
        }

        public InclusionSetting GetInclusionSetting(int exclusionId)
        {
            return _context.InclusionSettings.Find(exclusionId);
        }

        public IEnumerable<InclusionSetting> GetInclusionSettings()
        {
            return _context.InclusionSettings.ToList();
        }

        public void UpdateInfo(GalaxyInfo galaxyInfo)
        {
            _context.GalaxyInfo.Update(galaxyInfo);
        }

        public void AddObject(ArchiveObject archiveObject)
        {
            _context.ArchiveObjects.Add(archiveObject);
        }

        public void RemoveObject(ArchiveObject archiveObject)
        {
            _context.ArchiveObjects.Remove(archiveObject);
        }

        public void UpdateObject(ArchiveObject archiveObject)
        {
            _context.ArchiveObjects.Update(archiveObject);
        }

        public void AddEntry(ArchiveEntry archiveEntry)
        {
            _context.ArchiveEntries.Add(archiveEntry);
        }

        public void AddEvent(EventSetting eventSetting)
        {
            _context.OperationSettings.Add(eventSetting);
        }

        public void RemoveEvent(EventSetting eventSetting)
        {
            _context.OperationSettings.Remove(eventSetting);
        }

        public void AddInclusion(InclusionSetting inclusionSetting)
        {
            _context.InclusionSettings.Remove(inclusionSetting);
        }

        public void RemoveInclusion(InclusionSetting inclusionSetting)
        {
            _context.InclusionSettings.Remove(inclusionSetting);
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