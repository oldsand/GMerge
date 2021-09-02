using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GCommon.Archiving.Abstractions;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving
{
    public class ArchiveRepository : IArchiveRepository
    {
        private readonly ArchiveContext _context;

        public ArchiveRepository(string connectionString)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
        }

        public ArchiveConfig GetConfig()
        {
            return _context.Archive
                .Include(x => x.EventSettings)
                .Include(x => x.InclusionSettings)
                .Include(x => x.IgnoreSettings)
                .Single();
        }
        
        public bool IsArchivable(ArchiveObject archiveObject, Operation operation = null)
        {
            if (archiveObject == null) 
                throw new ArgumentNullException(nameof(archiveObject), "archiveObject can not be null");
            
            operation ??= archiveObject.Logs.OrderByDescending(x => x.ChangedOn).FirstOrDefault()?.Operation;
            
            if (operation == null)
                throw new InvalidOperationException("Could not find log operation for archive object");

            var config = GetConfig();
            var exists = ObjectExists(archiveObject.ObjectId);

            return config.HasInclusionFor(archiveObject.Template, archiveObject.IsTemplate, exists) 
                   && config.HasTriggerFor(operation);
        }

        public bool ObjectExists(int objectId)
        {
            return _context.Objects.Any(x => x.ObjectId == objectId);
        }

        public ArchiveObject GetObject(int objectId)
        {
            return _context.Objects
                .Include(x => x.Entries)
                .Include(x => x.Logs)
                .SingleOrDefault(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveObject> GetObjects()
        {
            return _context.Objects.ToList();
        }

        public IEnumerable<ArchiveObject> GetObjects(string tagName)
        {
            return _context.Objects.Where(x => x.TagName == tagName);
        }

        public void UpsertObject(ArchiveObject archiveObject)
        {
            if (_context.Objects.All(x => x.ObjectId != archiveObject.ObjectId))
            {
                _context.Entry(archiveObject).State = EntityState.Added;
                _context.Objects.Add(archiveObject);
                return;
            }

            _context.Objects.Update(archiveObject);
        }

        public void DeleteObject(int objectId)
        {
            var target = _context.Objects.Find(objectId);
            if (target == null) return;
            _context.Objects.Remove(target);
        }

        public IEnumerable<ArchiveEntry> GetEntries()
        {
            return _context.Entries.ToList();
        }

        public IEnumerable<ArchiveEntry> FindEntries(Expression<Func<ArchiveEntry, bool>> predicate)
        {
            return _context.Entries.Where(predicate).ToList();
        }

        public ArchiveLog GetLog(int changeLogId)
        {
            return _context.Logs
                .Include(x => x.ArchiveObject)
                .Include(x => x.Entry)
                .SingleOrDefault(x => x.ChangeLogId == changeLogId);
        }

        public IEnumerable<ArchiveLog> GetLogs()
        {
            return _context.Logs;
        }

        public IEnumerable<ArchiveLog> FindLogs(Expression<Func<ArchiveLog, bool>> predicate)
        {
            return _context.Logs.Where(predicate);
        }

        public QueuedLog GetQueuedLog(int changelogId)
        {
            return _context.Queue.Find(changelogId);
        }

        public void Enqueue(QueuedLog log)
        {
            var target = _context.Queue.Find(log.ChangeLogId);
            if (target != null) return;
            
            _context.Queue.Add(log);
        }

        public void Dequeue(int changeLogId)
        {
            var target = _context.Queue.Find(changeLogId);
            if (target == null) return;
            
            _context.Queue.Remove(target);
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
            _context.Dispose();
        }
    }
}