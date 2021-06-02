using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Client.Data
{
    public class ClientContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={ApplicationPath.ProgramData}.Client.db"); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientContext).Assembly);
        }
        
        public DbSet<GalaxyResource> Resources { get; set; }
    }
}