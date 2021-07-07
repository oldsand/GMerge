using System.IO;
using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GalaxyMerge.Archiving.Tests
{
    [TestFixture]
    public class ArchiveBuilderTests
    {
        [Test]
        public void Build_DefaultConfiguration_CreatesDatabaseWithExpectedData()
        {
            var config = ArchiveConfiguration.Default("GalaxyName");
            var builder = new ArchiveBuilder();
            
            builder.Build(config);

            var fileName = Path.Combine(ApplicationPath.Archives, "GalaxyName.db");
            FileAssert.Exists(fileName);

            var connectionString = DbStringBuilder.ArchiveString("GalaxyName");
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            var context = new ArchiveContext(options);
            
            var archive = context.Archive.Single();
            Assert.AreEqual("GalaxyName", archive.GalaxyName);
            Assert.AreEqual(ArchestraVersion.Sp2012R2P03, archive.Version);

            context.Database.EnsureDeleted();

            FileAssert.DoesNotExist(fileName);
        }
    }
}