using System.Linq;
using NUnit.Framework;

namespace GalaxyMerge.Archive.Tests
{
    [TestFixture]
    public class ArchiveConfigurationTests
    {
        [Test]
        public void Empty_WhenCalled_ReturnsCorrectConfiguration()
        {
            var config = ArchiveConfiguration.Empty("GalaxyName", 1, "123", "456");
            
            Assert.AreEqual("GalaxyName", config.GalaxyInfo.GalaxyName);
            Assert.AreEqual(1, config.GalaxyInfo.VersionNumber);
            Assert.AreEqual("123", config.GalaxyInfo.CdiVersion);
            Assert.AreEqual("456", config.GalaxyInfo.IsaVersion);
            Assert.AreEqual(@"Data Source=C:\ProgramData\GalaxyMerge\Archives\GalaxyName.db", config.ConnectionString);
            Assert.AreEqual(@"C:\ProgramData\GalaxyMerge\Archives\GalaxyName.db", config.FileName);
        }
        
        [Test]
        public void Default_WhenCalled_ReturnsCorrectConfiguration()
        {
            var config = ArchiveConfiguration.Default("GalaxyName", 1, "123", "456");
            
            Assert.AreEqual("GalaxyName", config.GalaxyInfo.GalaxyName);
            Assert.AreEqual(1, config.GalaxyInfo.VersionNumber);
            Assert.AreEqual("123", config.GalaxyInfo.CdiVersion);
            Assert.AreEqual("456", config.GalaxyInfo.IsaVersion);
            Assert.AreEqual(@"Data Source=C:\ProgramData\GalaxyMerge\Archives\GalaxyName.db", config.ConnectionString);
            Assert.AreEqual(@"C:\ProgramData\GalaxyMerge\Archives\GalaxyName.db", config.FileName);
            Assert.IsNotEmpty(config.ArchiveEvents);
            Assert.IsNotEmpty(config.ArchiveTemplates);
            
            Assert.True(config.ArchiveEvents.All(x => x.EventId >= 0));
            Assert.True(config.ArchiveEvents.All(x => x.EventName != ""));
            Assert.True(config.ArchiveTemplates.All(x => x.TemplateId > 0));
            Assert.True(config.ArchiveTemplates.All(x => x.TemplateName != ""));
        }
    }
}