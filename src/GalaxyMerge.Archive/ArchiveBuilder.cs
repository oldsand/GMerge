using System.IO;
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

            if (DatabaseFileExists(configurationBuilder.FileName)) return;

            context.Database.EnsureCreated();
            ApplyConfiguration(context, configurationBuilder);
        }

        private static void ApplyConfiguration(ArchiveContext context, ArchiveConfigurationBuilder configurationBuilder)
        {
            context.GalaxyInfo.Add(configurationBuilder.GalaxyInfo);
            context.EventSettings.AddRange(configurationBuilder.EventSettings);
            context.InclusionSettings.AddRange(configurationBuilder.InclusionSettings);
            context.SaveChanges();
        }

        private static bool DatabaseFileExists(string fileName)
        {
            return File.Exists(fileName);
        }
    }
}