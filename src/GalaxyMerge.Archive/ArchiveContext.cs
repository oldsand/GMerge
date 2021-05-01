using System.Runtime.CompilerServices;
using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;

[assembly:InternalsVisibleTo("GalaxyMerge.Archive.Tests")]
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
        
        public DbSet<ArchiveInfo> Info { get; set; }
        public DbSet<ArchiveObject> Objects { get; set; }
        public DbSet<ArchiveEntry> Entries { get; set; }
        public DbSet<ArchiveEvent> Events { get; set; }
        public DbSet<ArchiveTemplate> Exclusions { get; set; }
    }
}