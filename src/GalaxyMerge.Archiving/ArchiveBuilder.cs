using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archiving
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(IArchiveConfiguration config)
        {
            var archive = config.Build();
            
            var connectionString = DbStringBuilder.ArchiveString(archive.ArchiveName);
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(connectionString).Options;
            
            using var context = new ArchiveContext(options);
            context.Database.EnsureCreated();
            context.Archive.Add(config.Build());
            
            context.SaveChanges();
        }
    }
}