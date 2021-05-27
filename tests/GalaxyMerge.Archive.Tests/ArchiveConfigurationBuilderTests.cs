using System;
using System.Linq;
using GalaxyMerge.Primitives;
using NUnit.Framework;

namespace GalaxyMerge.Archive.Tests
{
    [TestFixture]
    public class ArchiveConfigurationBuilderTests
    {
        [Test]
        public void Empty_WhenCalled_ReturnsCorrectConfiguration()
        {
            var config = new ArchiveConfigurationBuilder("GalaxyName", 1, "123", "456");
            
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
            var config = ArchiveConfigurationBuilder.Default("GalaxyName", 1, "123", "456");
            
            Assert.AreEqual("GalaxyName", config.GalaxyInfo.GalaxyName);
            Assert.AreEqual(1, config.GalaxyInfo.VersionNumber);
            Assert.AreEqual("123", config.GalaxyInfo.CdiVersion);
            Assert.AreEqual("456", config.GalaxyInfo.IsaVersion);
            Assert.AreEqual(@"Data Source=C:\ProgramData\GalaxyMerge\Archives\GalaxyName.db", config.ConnectionString);
            Assert.AreEqual(@"C:\ProgramData\GalaxyMerge\Archives\GalaxyName.db", config.FileName);
            Assert.IsNotEmpty(config.EventSettings);
            Assert.IsNotEmpty(config.InclusionSettings);
            
            Assert.True(config.EventSettings.All(x => x.Operation.Name != ""));
            Assert.True(config.InclusionSettings.All(x => x.Template.Id > 0));
            Assert.True(config.InclusionSettings.All(x => x.Template.Name != ""));
        }

        [Test]
        public void OverrideGalaxyInfo_WhenCalled_ReturnsConfigWithExpectedValues()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.HasInfo("SomeOtherGalaxy", 4, "654", "987654");
            
            Assert.AreEqual(config.GalaxyInfo.GalaxyName, "SomeOtherGalaxy");
            Assert.AreEqual(config.GalaxyInfo.VersionNumber, 4);
            Assert.AreEqual(config.GalaxyInfo.CdiVersion, "654");
            Assert.AreEqual(config.GalaxyInfo.IsaVersion, "987654");
        }
        
        [Test]
        public void OverrideConnectionString_WhenCalled_AreEqualOverriddenConnectionString()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.HasConnectionString(@"Data Source=C:\SomeGalaxy.db");
            
            Assert.AreEqual(config.ConnectionString, @"Data Source=C:\SomeGalaxy.db");
        }

        [Test]
        public void SetTrigger_ValidOperation_IsArchiveTriggerTrue()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetTrigger(Operation.AssignFailed);
            
            var result = config.EventSettings.Single(x => x.Operation == Operation.AssignFailed);
            Assert.True(result.IsArchiveTrigger);
        }
        
        [Test]
        public void SetTrigger_NullReference_ThrowsArgumentException()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            Assert.Throws<ArgumentNullException>(() => config = config.SetTrigger(null));
        }
        
        [Test]
        public void SetTrigger_ChainCalls_AllReturnIsArchiveTriggerTrue()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetTrigger(Operation.AssignFailed)
                .SetTrigger(Operation.Rename)
                .SetTrigger(Operation.CreateInstance);
            
            var assignedFailed = config.EventSettings.Single(x => x.Operation == Operation.AssignFailed);
            var rename = config.EventSettings.Single(x => x.Operation == Operation.Rename);
            var createInstance = config.EventSettings.Single(x => x.Operation == Operation.CreateInstance);
            
            Assert.True(assignedFailed.IsArchiveTrigger);
            Assert.True(rename.IsArchiveTrigger);
            Assert.True(createInstance.IsArchiveTrigger);
        }
        
        [Test]
        public void SetTrigger_IsArchiveTriggerFalse_ReturnsFalse()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetTrigger(Operation.AssignFailed, false);
            
            var result = config.EventSettings.Single(x => x.Operation == Operation.AssignFailed);
            Assert.False(result.IsArchiveTrigger);
        }
        
        [Test]
        public void SetTrigger_ChainCallsIsArchiveTriggerFalse_AllIsArchiveTriggerReturnFalse()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetTrigger(Operation.AssignFailed, false)
                .SetTrigger(Operation.Rename, false)
                .SetTrigger(Operation.CreateInstance, false);
            
            var assignedFailed = config.EventSettings.Single(x => x.Operation == Operation.AssignFailed);
            var rename = config.EventSettings.Single(x => x.Operation == Operation.Rename);
            var createInstance = config.EventSettings.Single(x => x.Operation == Operation.CreateInstance);
            
            Assert.False(assignedFailed.IsArchiveTrigger);
            Assert.False(rename.IsArchiveTrigger);
            Assert.False(createInstance.IsArchiveTrigger);
        }
        
        [Test]
        public void SetInclusion_ValidArguments_ReturnsExpectedValues()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetInclusion(Template.Area);
            
            var result = config.InclusionSettings.Single(x => x.Template == Template.Area);
            Assert.AreEqual(result.InclusionOption, InclusionOption.All);
            Assert.False(result.IncludesInstances);
        }
        
        [Test]
        public void SetInclusion_NullReference_ThrowsArgumentException()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            Assert.Throws<ArgumentNullException>(() => config = config.SetInclusion(null));
        }
        
        [Test]
        public void SetInclusion_ChainCalls_AllReturnExpectedValues()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetInclusion(Template.Galaxy, InclusionOption.None)
                .SetInclusion(Template.AppEngine, InclusionOption.Select)
                .SetInclusion(Template.ViewEngine, InclusionOption.All);
            
            var galaxy = config.InclusionSettings.Single(x => x.Template == Template.Galaxy);
            var appEngine = config.InclusionSettings.Single(x => x.Template == Template.AppEngine);
            var viewEngine = config.InclusionSettings.Single(x => x.Template == Template.ViewEngine);
            
            Assert.AreEqual(galaxy.InclusionOption, InclusionOption.None);
            Assert.AreEqual(appEngine.InclusionOption, InclusionOption.Select);
            Assert.AreEqual(viewEngine.InclusionOption, InclusionOption.All);
        }
        
        [Test]
        public void SetInclusion_IncludeInstances_ReturnsIncludeInstancesTrue()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetInclusion(Template.UserDefined, InclusionOption.All, true);
            
            var result = config.InclusionSettings.Single(x => x.Template == Template.UserDefined);
            Assert.True(result.IncludesInstances);
        }
        
        [Test]
        public void SetInclusion_ChainOverrideCalls_ReturnsExpected()
        {
            var config = new ArchiveConfigurationBuilder("Galaxy", 1, "123", "456");

            config = config.SetInclusion(Template.Galaxy, InclusionOption.None, true)
                .SetInclusion(Template.AppEngine, InclusionOption.Select, true)
                .SetInclusion(Template.ViewEngine, InclusionOption.All, true);
            
            var galaxy = config.InclusionSettings.Single(x => x.Template == Template.Galaxy);
            var appEngine = config.InclusionSettings.Single(x => x.Template == Template.AppEngine);
            var viewEngine = config.InclusionSettings.Single(x => x.Template == Template.ViewEngine);
            
            Assert.AreEqual(galaxy.InclusionOption, InclusionOption.None);
            Assert.AreEqual(appEngine.InclusionOption, InclusionOption.Select);
            Assert.AreEqual(viewEngine.InclusionOption, InclusionOption.All);
            Assert.True(galaxy.IncludesInstances);
            Assert.True(appEngine.IncludesInstances);
            Assert.True(viewEngine.IncludesInstances);
        }
    }
}