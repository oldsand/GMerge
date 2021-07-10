using GCommon.Archiving.Abstractions;
using GCommon.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(ArchiveConfiguration config)
        {
            var archive = config.GenerateArchive();
            var connectionString = config.GetConnectionString();
            
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            using var context = new ArchiveContext(options);
            
            context.Database.EnsureCreated();
            context.Archive.Add(archive);
            
            context.SaveChanges();
        }
    }
}