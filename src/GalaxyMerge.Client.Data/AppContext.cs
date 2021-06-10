using System.Runtime.CompilerServices;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("GalaxyMerge.Client.Data.Tests")]
namespace GalaxyMerge.Client.Data
{
    internal class AppContext : DbContext
    {
        public AppContext()
        {
        }
        
        public AppContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={ApplicationPath.ProgramData}\\app.db"); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
        }
        
        public DbSet<GalaxyResource> Resources { get; set; }
        public DbSet<LogEntry> Logs { get; set; }
        
    }
}