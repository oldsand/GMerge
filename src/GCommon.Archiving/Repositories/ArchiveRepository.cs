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
            Queue = new QueueRepository(_context);
        }

        public ArchiveRepository(string connectionString,
            IEventSettingsRepository events, 
            IInclusionSettingsRepository inclusions,
            IArchiveObjectRepository objects,
            IQueueRepository queue)
        {
            Events = events;
            Inclusions = inclusions;
            Objects = objects;
            Queue = queue;
        }
        
        public IEventSettingsRepository Events { get; }
        public IInclusionSettingsRepository Inclusions { get; }
        public IArchiveObjectRepository Objects { get; }
        public IQueueRepository Queue { get; }

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
        
        public bool CanArchive(ArchiveObject archiveObject, Operation operation)
        {
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