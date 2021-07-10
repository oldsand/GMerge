using System;
using System.Linq;
using GCommon.Primitives;
using GCommon.Archiving;
using NUnit.Framework;

namespace GalaxyMerge.Archiving.Tests
{
    [TestFixture]
    public class ArchiveConfigurationTests
    {
        [Test]
        public void Default_WhenCalled_ReturnsNotNull()
        {
            var config = ArchiveConfiguration.Default("GalaxyName");

            Assert.NotNull(config);
        }

        [Test]
        public void GenerateConfig_FromDefault_ReturnsExpectedGalaxyNameAndVersion()
        {
            var config = ArchiveConfiguration.Default("GalaxyName");

            var archive = config.GenerateArchive();

            Assert.NotNull(archive);
            Assert.AreEqual("GalaxyName", archive.GalaxyName);
            Assert.AreEqual(ArchestraVersion.SystemPlatform2012R2P3, archive.Version);
        }

        [Test]
        public void GenerateConfig_FromDefault_ReturnsExpectedEventSettings()
        {
            var config = ArchiveConfiguration.Default("GalaxyName");

            var archive = config.GenerateArchive();

            Assert.IsNotEmpty(archive.EventSettings);
            Assert.True(archive.EventSettings.All(x => x.Operation.Name != ""));

            var operations = archive.EventSettings.Where(x => x.IsArchiveEvent).Select(x => x.Operation).ToList();
            Assert.That(operations, Contains.Item(Operation.CheckInSuccess));
            Assert.That(operations, Contains.Item(Operation.CreateDerivedTemplate));
            Assert.That(operations, Contains.Item(Operation.CreateInstance));
            Assert.That(operations, Contains.Item(Operation.Rename));
            Assert.That(operations, Contains.Item(Operation.RenameTagName));
            Assert.That(operations, Contains.Item(Operation.RenameContainedName));
            Assert.That(operations, Contains.Item(Operation.Upload));
        }

        [Test]
        public void GenerateConfig_FromDefault_ReturnsExpectedInclusionSettings()
        {
            var config = ArchiveConfiguration.Default("GalaxyName");

            var archive = config.GenerateArchive();

            Assert.IsNotEmpty(archive.InclusionSettings);
            Assert.True(archive.InclusionSettings.All(x => x.Template.Name != ""));

            var templates = archive.InclusionSettings.Where(x => x.InclusionOption == InclusionOption.All)
                .Select(x => x.Template).ToList();
            Assert.That(templates, Contains.Item(Template.UserDefined));
            Assert.That(templates, Contains.Item(Template.Symbol));
        }

        [Test]
        public void ConfigureEvent_ValidOperations_ReturnsExpectedArchiveEventSetting()
        {
            var config = ArchiveConfiguration.Default("Galaxy")
                .ConfigureEvent(Operation.AssignSuccess)
                .ConfigureEvent(Operation.SaveObject, false)
                .ConfigureEvent(Operation.PublishSuccess)
                .ConfigureEvent(Operation.CreateInstance, false);

            var archive = config.GenerateArchive();
            var assignSuccess = archive.EventSettings.Single(x => x.Operation == Operation.AssignSuccess);
            var saveObject = archive.EventSettings.Single(x => x.Operation == Operation.SaveObject);
            var publishSuccess = archive.EventSettings.Single(x => x.Operation == Operation.PublishSuccess);
            var createInstance = archive.EventSettings.Single(x => x.Operation == Operation.CreateInstance);
            
            Assert.True(assignSuccess.IsArchiveEvent);
            Assert.False(saveObject.IsArchiveEvent);
            Assert.True(publishSuccess.IsArchiveEvent);
            Assert.False(createInstance.IsArchiveEvent);
        }

        [Test]
        public void ConfigureEvent_NullReference_ThrowsArgumentException()
        {
            var config = ArchiveConfiguration.Default("Galaxy");

            Assert.Throws<ArgumentNullException>(() => config = config.ConfigureEvent(null));
        }

        [Test]
        public void ConfigureInclusion_ValidArguments_ReturnsExpectedValues()
        {
            var config = ArchiveConfiguration.Default("Galaxy")
                .ConfigureInclusion(Template.Area)
                .ConfigureInclusion(Template.UserDefined, InclusionOption.Select, true)
                .ConfigureInclusion(Template.Symbol, InclusionOption.None)
                .ConfigureInclusion(Template.AppEngine, InclusionOption.All, true)
                .ConfigureInclusion(Template.ViewEngine, includeInstances: true);

            var archive = config.GenerateArchive();

            var area = archive.InclusionSettings.Single(x => x.Template == Template.Area);
            Assert.AreEqual(area.InclusionOption, InclusionOption.All);
            Assert.False(area.IncludeInstances);

            var userDefined = archive.InclusionSettings.Single(x => x.Template == Template.UserDefined);
            Assert.AreEqual(userDefined.InclusionOption, InclusionOption.Select);
            Assert.True(userDefined.IncludeInstances);

            var symbol = archive.InclusionSettings.Single(x => x.Template == Template.Symbol);
            Assert.AreEqual(symbol.InclusionOption, InclusionOption.None);
            Assert.False(symbol.IncludeInstances);

            var appEngine = archive.InclusionSettings.Single(x => x.Template == Template.AppEngine);
            Assert.AreEqual(appEngine.InclusionOption, InclusionOption.All);
            Assert.True(appEngine.IncludeInstances);

            var viewEngine = archive.InclusionSettings.Single(x => x.Template == Template.ViewEngine);
            Assert.AreEqual(viewEngine.InclusionOption, InclusionOption.All);
            Assert.True(viewEngine.IncludeInstances);
        }

        [Test]
        public void ConfigureInclusion_NullReference_ThrowsArgumentException()
        {
            var config = ArchiveConfiguration.Default("Galaxy");

            Assert.Throws<ArgumentNullException>(() => config = config.ConfigureInclusion(null));
        }
    }
}