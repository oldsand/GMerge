using System.IO;
using System.Linq;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
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
            var archive = new ArchiveConfig("GalaxyName");
            var builder = new ArchiveBuilder();
            
            builder.Build(archive);

            var fileName = Path.Combine(ApplicationPath.Archives, "GalaxyName.db");
            FileAssert.Exists(fileName);

            var connectionString = DbStringBuilder.ArchiveString("GalaxyName");
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            var context = new ArchiveContext(options);
            
            var result = context.Archive.Single();
            Assert.AreEqual("GalaxyName", result.GalaxyName);
            Assert.AreEqual(ArchestraVersion.SystemPlatform2012R2P3, result.Version);

            context.Database.EnsureDeleted();

            FileAssert.DoesNotExist(fileName);
        }
    }
}