using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archiving.Repositories
{
    public class ArchiveRepository : IArchiveRepository
    {
        private readonly ArchiveContext _context;

        public ArchiveRepository(string connectionString)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
            Objects = new ArchiveObjectRepository(_context);
            Queue = new QueueRepository(_context);
        }

        public Archive GetArchive()
        {
            return _context.Archive
                .Include(x => x.EventSettings)
                .Include(x => x.InclusionSettings)
                .Include(x => x.IgnoreSettings)
                .Include(x => x.Objects)
                .Single();
        }

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

        public IArchiveObjectRepository Objects { get; }
        public IQueueRepository Queue { get; }

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