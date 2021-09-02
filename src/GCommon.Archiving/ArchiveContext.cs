using System.Runtime.CompilerServices;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using Microsoft.EntityFrameworkCore;

[assembly:InternalsVisibleTo("GCommon.Archiving.IntegrationTests")]
namespace GCommon.Archiving
{
    internal class ArchiveContext : DbContext
    {
        public ArchiveContext(DbContextOptions<ArchiveContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={ApplicationPath.Archives}.Default.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArchiveContext).Assembly);
        }

        public DbSet<ArchiveConfig> Archive { get; set; }
        public DbSet<ArchiveObject> Objects { get; set; }
        public DbSet<ArchiveEntry> Entries { get; set; }
        public DbSet<ArchiveLog> Logs { get; set; }
        public DbSet<QueuedLog> Queue { get; set; }
    }
}