using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GalaxyMerge.Archive.Tests
{
    [TestFixture]
    public class ArchiveBuilderTests
    {
        [Test]
        public void Build_EmptyConfiguration_CreatesDatabaseWithExpectedData()
        {
            var config = ArchiveConfiguration.Empty("SomeGalaxy", 1, "1234", "5678");
            ArchiveBuilder.Build(config);

            FileAssert.Exists(config.FileName);
            
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(config.ConnectionString).Options;
            var context = new ArchiveContext(options);
            var info = context.Info.Single();
            Assert.AreEqual("SomeGalaxy", info.GalaxyName);
            Assert.AreEqual(1, info.VersionNumber);
            Assert.AreEqual("1234", info.CdiVersion);
            Assert.AreEqual("5678", info.IsaVersion);

            context.Database.EnsureDeleted();

            FileAssert.DoesNotExist(config.FileName);
        }
    }
}