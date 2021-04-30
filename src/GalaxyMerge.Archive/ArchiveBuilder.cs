using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    public static class ArchiveBuilder
    {
        public static void Build(ArchiveConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(configuration.ConnectionString).Options;
            
            using var context = new ArchiveContext(options);

            context.Database.EnsureCreated();
            ApplyConfiguration(context, configuration);
        }

        private static void ApplyConfiguration(ArchiveContext context, ArchiveConfiguration configuration)
        {
            context.Info.Add(configuration.ArchiveInfo);
            context.Events.AddRange(configuration.ArchiveEvents);
            context.Exclusions.AddRange(configuration.ArchiveExclusions);
            context.SaveChanges();
        }
    }
}