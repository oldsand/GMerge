using GCommon.Archiving.Abstractions;
using GCommon.Core;
using GCommon.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(ArchiveConfig archiveConfig, string connectionString = null)
        {
            connectionString ??= DbStringBuilder.ArchiveString(archiveConfig.GalaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            using var context = new ArchiveContext(options);
            
            context.Database.EnsureCreated();
            context.Archive.Add(archiveConfig);
            
            context.SaveChanges();
        }
    }
}