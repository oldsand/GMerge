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
            var config = new ArchiveConfigurationBuilder("SomeGalaxy", 1, "1234", "5678");
            var builder = new ArchiveBuilder();
            builder.Build(config);

            FileAssert.Exists(config.FileName);
            
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(config.ConnectionString).Options;
            var context = new ArchiveContext(options);
            var info = context.GalaxyInfo.Single();
            Assert.AreEqual("SomeGalaxy", info.GalaxyName);
            Assert.AreEqual(1, info.VersionNumber);
            Assert.AreEqual("1234", info.CdiVersion);
            Assert.AreEqual("5678", info.IsaVersion);

            context.Database.EnsureDeleted();

            FileAssert.DoesNotExist(config.FileName);
        }
        
        [Test]
        public void Build_DefaultConfiguration_CreatesDatabaseWithExpectedData()
        {
            var config =  ArchiveConfigurationBuilder.Default("SomeGalaxy", 1, "1234", "5678");
            var builder = new ArchiveBuilder();
            builder.Build(config);

            FileAssert.Exists(config.FileName);
            
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(config.ConnectionString).Options;
            var context = new ArchiveContext(options);
            var info = context.GalaxyInfo.Single();
            Assert.AreEqual("SomeGalaxy", info.GalaxyName);
            Assert.AreEqual(1, info.VersionNumber);
            Assert.AreEqual("1234", info.CdiVersion);
            Assert.AreEqual("5678", info.IsaVersion);

            var eventSettings = config.EventSettings;
            Assert.IsNotEmpty(eventSettings);
            
            var inclusionSettings = config.InclusionSettings;
            Assert.IsNotEmpty(inclusionSettings);

            context.Database.EnsureDeleted();

            FileAssert.DoesNotExist(config.FileName);
        }
    }
}