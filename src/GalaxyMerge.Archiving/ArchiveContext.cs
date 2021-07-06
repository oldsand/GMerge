using System.Runtime.CompilerServices;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

[assembly:InternalsVisibleTo("GalaxyMerge.Archiving.Tests")]
namespace GalaxyMerge.Archiving
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

        public DbSet<Archive> Archive { get; set; }
        public DbSet<ArchiveObject> Objects { get; set; }
        public DbSet<QueuedEntry> Queue { get; set; }
    }
}