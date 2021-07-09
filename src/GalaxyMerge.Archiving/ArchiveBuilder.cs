using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archiving
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(ArchiveConfiguration config)
        {
            var archive = config.GenerateArchive();
            
            var connectionString = DbStringBuilder.ArchiveString(archive.GalaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(connectionString).Options;
            using var context = new ArchiveContext(options);
            
            context.Database.EnsureCreated();
            context.Archive.Add(archive);
            
            context.SaveChanges();
        }
    }
}