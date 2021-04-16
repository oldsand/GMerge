using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    internal class ArchiveContext : DbContext
    {
        public ArchiveContext(DbContextOptions<ArchiveContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArchiveContext).Assembly);
        }
        
        public DbSet<ArchiveEntry> Entries { get; set; }
        public DbSet<ArchiveInfo> Info { get; set; }
    }
}