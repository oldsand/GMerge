using System;
using System.Linq;
using FluentAssertions;
using GCommon.Primitives.Enumerations;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests
{
    [TestFixture]
    public class ArchiveConfigTests
    {
        [Test]
        public void Constructor_GalaxyName_ArchiveShouldNotBeNull()
        {
            var config = new ArchiveConfig("GalaxyName");

            Assert.NotNull(config);
        }

        [Test]
        public void Constructor_GalaxyName_ShouldHaveExpectedDefaultNameAndVersion()
        {
            var archive = new ArchiveConfig("GalaxyName");
            
            Assert.AreEqual("GalaxyName", archive.GalaxyName);
            Assert.AreEqual(ArchestraVersion.SystemPlatform2012R2P3, archive.Version);
        }

        [Test]
        public void Constructor_GalaxyName_ShouldHaveExpectedDefaultEventSettings()
        {
            var archive = new ArchiveConfig("GalaxyName");

            archive.EventSettings.Should().NotBeEmpty();
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
            var archive = new ArchiveConfig("GalaxyName");

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
            var archive = new ArchiveConfig("GalaxyName")
                .Configure(Operation.AssignSuccess)
                .Configure(Operation.SaveObject, false)
                .Configure(Operation.PublishSuccess)
                .Configure(Operation.CreateInstance, false);
            
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
            var archive = new ArchiveConfig("GalaxyName");

            Assert.Throws<ArgumentNullException>(() => archive = archive.Configure(operation: null));
        }

        [Test]
        public void ConfigureInclusion_ValidArguments_ReturnsExpectedValues()
        {
            var archive = new ArchiveConfig("GalaxyName")
                .Configure(Template.Area)
                .Configure(Template.UserDefined, InclusionOption.Select, true)
                .Configure(Template.Symbol, InclusionOption.None)
                .Configure(Template.AppEngine, InclusionOption.All, true)
                .Configure(Template.ViewEngine, includeInstances: true);

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
            var archive = new ArchiveConfig("GalaxyName");

            Assert.Throws<ArgumentNullException>(() => archive = archive.Configure(template: null));
        }
        
        [Test]
        public void HasTriggerFor_OperationThatIsArchiveEvent_ShouldBeTrue()
        {
            var archive = new ArchiveConfig("GalaxyName");

            var result = archive.HasTriggerFor(Operation.CreateInstance);

            result.Should().BeTrue();
        }
        
        [Test]
        public void HasTriggerFor_OperationThatIsNotArchiveEvent_ShouldBeFalse()
        {
            var archive = new ArchiveConfig("GalaxyName");

            var result = archive.HasTriggerFor(Operation.DeploySuccess);

            result.Should().BeFalse();
        }
        
        [Test]
        public void HasTriggerFor_Null_ShouldThrowArgumentNullException()
        {
            var archive = new ArchiveConfig("GalaxyName");
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = archive.HasTriggerFor(null);
            }, "operation can not be null");
        }
        
        [Test]
        public void GetEventFor_ValidOperation_ResultShouldBeExpectedOperation()
        {
            var archive = new ArchiveConfig("GalaxyName");

            var result = archive.GetEventFor(Operation.DeploySuccess);

            result.Should().NotBeNull();
            result.Operation.Should().Be(Operation.DeploySuccess);
        }
        
        [Test]
        public void GetEventFor_Null_ShouldThrowArgumentNullException()
        {
            var archive = new ArchiveConfig("GalaxyName");

            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = archive.GetEventFor(null);
            }, "operation can not be null");
        }
        
        [Test]
        public void EventSettings_Count_ShouldBeEqualToOperationsEnumCount()
        {
            var archive = new ArchiveConfig("GalaxyName");
            var count = Operation.List.Count;

            var eventSettings = archive.EventSettings.ToList();

            eventSettings.Should().HaveCount(count);
        }

        [Test]
        public void GetArchiveEvents_WhenCalled_ReturnsAllArchiveEvents()
        {
            var archive = new ArchiveConfig("GalaxyName");
            
            var operations = archive.GetArchiveEvents().ToList();
            
            Assert.True(operations.All(x => x.IsArchiveEvent));
        }
        
        [Test]
        public void GetInclusionFor_ValidTemplate_ShouldReturnExpectedTemplate()
        {
            var archive = new ArchiveConfig("GalaxyName");

            var result = archive.GetInclusionFor(Template.Galaxy);

            result.Should().NotBeNull();
            result.Template.Should().Be(Template.Galaxy);
        }
        
        [Test]
        public void GetInclusionFor_Null_ThrowArgumentNullException()
        {
            var archive = new ArchiveConfig("GalaxyName");

            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = archive.GetInclusionFor(null);
            }, "archiveObject can not be null");
        }
        
        [Test]
        public void InclusionSettings_Count_ShouldBeEqualToTemplateEnumCount()
        {
            var archive = new ArchiveConfig("GalaxyName");
            var count = Template.List.Count;

            var result = archive.InclusionSettings.ToList().Count;

            result.Should().Be(count);
        }

        [Test]
        public void FindByOption_OptionsAll_ReturnsAllInclusionsWithAll()
        {
            var archive = new ArchiveConfig("GalaxyName");
            
            var settings = archive.InclusionsWithOption(InclusionOption.All).ToList();

            settings.Should().HaveCount(2);
        }
    }
}