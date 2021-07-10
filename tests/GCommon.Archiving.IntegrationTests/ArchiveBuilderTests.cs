using System.IO;
using System.Linq;
using GCommon.Core.Utilities;

using GCommon.Archiving;
using GCommon.Primitives;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
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
            var options = new DbContextOptionsBuilder<Archiving.ArchiveContext>().UseSqlite(connectionString).Options;
            var context = new Archiving.ArchiveContext(options);
            
            var archive = context.Archive.Single();
            Assert.AreEqual("GalaxyName", archive.GalaxyName);
            Assert.AreEqual(ArchestraVersion.SystemPlatform2012R2P3, archive.Version);

            context.Database.EnsureDeleted();

            FileAssert.DoesNotExist(fileName);
        }
    }
}