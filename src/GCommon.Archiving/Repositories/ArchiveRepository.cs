using System;
using System.Linq;
using System.Threading.Tasks;
using GCommon.Primitives;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving.Repositories
{
    public class ArchiveRepository : IArchiveRepository
    {
        private readonly ArchiveContext _context;

        public ArchiveRepository(string connectionString)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
            
            Events = new EventSettingRepository(_context);
            Inclusions = new InclusionSettingsRepository(_context);
            Objects = new ArchiveObjectRepository(_context);
            Logs = new ArchiveLogRepository(_context);
            Queue = new QueuedLogRepository(_context);
        }

        public ArchiveRepository(string connectionString,
            IEventSettingsRepository events, 
            IInclusionSettingsRepository inclusions,
            IArchiveObjectRepository objects,
            IArchiveLogRepository logs,
            IQueuedLogRepository queue)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
            
            Events = events;
            Inclusions = inclusions;
            Objects = objects;
            Logs = logs;
            Queue = queue;
        }
        
        public IEventSettingsRepository Events { get; }
        public IInclusionSettingsRepository Inclusions { get; }
        public IArchiveObjectRepository Objects { get; }
        public IArchiveLogRepository Logs { get; }
        public IQueuedLogRepository Queue { get; }

        public Archive GetArchiveInfo()
        {
            return _context.Archive.Single();
        }

        public Archive GetArchiveSettings()
        {
            return _context.Archive
                .Include(x => x.EventSettings)
                .Include(x => x.InclusionSettings)
                .Include(x => x.IgnoreSettings)
                .Single();
        }
        
        public bool IsArchivable(ArchiveObject archiveObject)
        {
            var operation = archiveObject.Logs.OrderByDescending(x => x.ChangedOn).FirstOrDefault()?.Operation;
            if (operation == null)
                throw new InvalidOperationException("Could not find log operation for archive object");
            
            return Inclusions.IsIncluded(archiveObject) && Events.IsTrigger(operation);
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