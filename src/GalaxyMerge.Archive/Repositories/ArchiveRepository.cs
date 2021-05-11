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
        
        public GalaxyInfo GetGalaxyInfo()
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

        public IEnumerable<ArchiveObject> GetAllObjects()
        {
            return _context.ArchiveObjects.ToList();
        }

        public ArchiveObject GetObjectIncludeEntries(int objectId)
        {
            return _context.ArchiveObjects.Include(x => x.Entries).SingleOrDefault(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveObject> FindAllByTagName(string tagName)
        {
            return _context.ArchiveObjects.Where(x => x.TagName == tagName);
        }

        public bool HasEntries(int objectId)
        {
            return _context.ArchiveEntries.Any(x => x.ObjectId == objectId);
        }

        public ArchiveEntry GetLatestEntry(int objectId)
        {
            return _context.ArchiveEntries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveEntry> GetAllEntries()
        {
            return _context.ArchiveEntries.ToList();
        }

        public EventSetting GetEventSetting(int operationId)
        {
            return _context.EventSettings.SingleOrDefault(x => x.OperationId == operationId);
        }

        public IEnumerable<EventSetting> GetEventSettings()
        {
            return _context.EventSettings.ToList();
        }

        public InclusionSetting GetInclusionSetting(int templateId)
        {
            return _context.InclusionSettings.SingleOrDefault(x => x.TemplateId == templateId);
        }

        public IEnumerable<InclusionSetting> GetInclusionSettings()
        {
            return _context.InclusionSettings.ToList();
        }

        public void AddObject(ArchiveObject archiveObject)
        {
            _context.ArchiveObjects.Add(archiveObject);
        }

        public void RemoveObject(ArchiveObject archiveObject)
        {
            _context.ArchiveObjects.Remove(archiveObject);
        }

        public void RemoveObject(int objectId)
        {
            var target = _context.ArchiveObjects.Find(objectId);
            _context.Remove(target);
        }

        public void UpdateObject(ArchiveObject archiveObject)
        {
            _context.ArchiveObjects.Update(archiveObject);
        }

        public void UpdateGalaxyInfo(GalaxyInfo galaxyInfo)
        {
            _context.GalaxyInfo.Update(galaxyInfo);
        }
        
        public void UpdateEventSettings(IEnumerable<EventSetting> eventSettings)
        {
            _context.EventSettings.UpdateRange(eventSettings);
        }
        
        public void UpdateInclusionSettings(IEnumerable<InclusionSetting> inclusionSettings)
        {
            _context.InclusionSettings.UpdateRange(inclusionSettings);
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