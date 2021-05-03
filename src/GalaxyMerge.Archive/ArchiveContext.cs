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
        
        public DbSet<GalaxyInfo> GalaxyInfo { get; set; }
        public DbSet<ArchiveObject> ArchiveObjects { get; set; }
        public DbSet<ArchiveEntry> ArchiveEntries { get; set; }
        public DbSet<EventSetting> EventSettings { get; set; }
        public DbSet<InclusionSetting> InclusionSettings { get; set; }
    }
}