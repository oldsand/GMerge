using GCommon.Archiving.Abstractions;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving
{
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(Archive archive, string connectionString = null)
        {
            connectionString ??= DbStringBuilder.ArchiveString(archive.GalaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            using var context = new ArchiveContext(options);
            
            context.Database.EnsureCreated();
            context.Archive.Add(archive);
            
            context.SaveChanges();
        }
    }
}