using System.Runtime.CompilerServices;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

[assembly:InternalsVisibleTo("GalaxyMerge.Archive.Tests")]
namespace GalaxyMerge.Archive
{
    internal class ArchiveContext : DbContext
    {
        public ArchiveContext()
        {
        }

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
        
        public DbSet<ArchiveEntry> ArchiveEntries { get; set; }
        public DbSet<ArchiveObject> ArchiveObjects { get; set; }
        public DbSet<GalaxyInfo> GalaxyInfo { get; set; }
        public DbSet<InclusionSetting> InclusionSettings { get; set; }
        public DbSet<EventSetting> EventSettings { get; set; }
        
    }
}