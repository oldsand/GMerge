using GalaxyMerge.Archive.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(ArchiveConfigurationBuilder configurationBuilder)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(configurationBuilder.ConnectionString).Options;
            using var context = new ArchiveContext(options);

            context.Database.EnsureCreated();
            
            ApplyConfiguration(context, configurationBuilder);
        }

        public void Configure(ArchiveConfigurationBuilder configurationBuilder)
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(configurationBuilder.ConnectionString).Options;
            using var context = new ArchiveContext(options);
            
            ApplyConfiguration(context, configurationBuilder);
        }

        private static void ApplyConfiguration(ArchiveContext context, ArchiveConfigurationBuilder configurationBuilder)
        {
            context.GalaxyInfo.Add(configurationBuilder.GalaxyInfo);
            context.OperationSettings.AddRange(configurationBuilder.OperationSettings);
            context.InclusionSettings.AddRange(configurationBuilder.InclusionSettings);
            context.SaveChanges();
        }
    }
}