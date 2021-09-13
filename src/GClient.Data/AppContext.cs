using System.Runtime.CompilerServices;
using GClient.Data.Entities;
using GCommon.Utilities;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("GClient.Data.UnitTests")]
namespace GClient.Data
{
    public class AppContext : DbContext
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
        
        public DbSet<ResourceEntry> Resources { get; set; }
        public DbSet<LogEntry> Logs { get; set; }
        
    }
}