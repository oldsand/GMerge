using GalaxyMerge.Archive.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(ArchiveConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(configuration.ConnectionString).Options;
            
            using var context = new ArchiveContext(options);

            context.Database.EnsureCreated();
            
            ApplyConfiguration(context, configuration);
        }

        private static void ApplyConfiguration(ArchiveContext context, ArchiveConfiguration configuration)
        {
            context.GalaxyInfo.Add(configuration.GalaxyInfo);
            context.EventSettings.AddRange(configuration.ArchiveEvents);
            context.InclusionSettings.AddRange(configuration.ArchiveTemplates);
            context.SaveChanges();
        }
    }
}