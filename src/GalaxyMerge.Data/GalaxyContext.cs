using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data
{
    internal class GalaxyContext : DbContext
    {
        public GalaxyContext(DbContextOptions<GalaxyContext> options) : base(options)
        {
        }

        public virtual DbSet<GObject> Objects { get; set; }
        public virtual DbSet<TemplateDefinition> TemplateDefinitions { get; set; }
        public virtual DbSet<PrimitiveDefinition> PrimitiveDefinitions { get; set; }
        public virtual DbSet<UserProfile> Users { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public virtual DbSet<ObjectLookup> ObjectLookups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GalaxyContext).Assembly);
        }
        
        public static GalaxyContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<GalaxyContext>().UseSqlServer(connectionString).Options;
            return new GalaxyContext(options);
        }
    }
}