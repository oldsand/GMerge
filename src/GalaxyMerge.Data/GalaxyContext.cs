using System.Data.Common;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data
{
    public class GalaxyContext : DbContext
    {
        private readonly DbConnection _connection;

        public GalaxyContext(DbContextOptions<GalaxyContext> options, DbConnection connection = null) : base(options)
        {
            _connection = connection;
        }

        public virtual DbSet<GObject> Objects { get; set; }
        public virtual DbSet<TemplateDefinition> TemplateDefinitions { get; set; }
        public virtual DbSet<PrimitiveDefinition> PrimitiveDefinitions { get; set; }
        public virtual DbSet<UserProfile> Users { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GalaxyContext).Assembly);
        }
    }
}