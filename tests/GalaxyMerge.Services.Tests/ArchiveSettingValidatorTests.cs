using System.IO;
using GalaxyMerge.Archive;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Primitives;
using GalaxyMerge.Testing;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ArchiveSettingValidatorTests
    { 
        [SetUp]
        public void Setup()
        {
            var builder = new ArchiveBuilder();
            builder.Build(ArchiveConfigurationBuilder.Default(Settings.CurrentTestGalaxy));

            using var objectRepo = new ObjectRepository(Settings.CurrentTestGalaxy);
            var gObject = objectRepo.FindByTagName("$Site_Data");

            var testObject = new ArchiveObject(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion,
                Enumeration.FromId<Template>(gObject.TemplateId));
            testObject.AddEntry(new byte[100]);
            
            using var archiveRepo = new ArchiveRepository(Settings.CurrentTestGalaxy);
            archiveRepo.AddObject(testObject);
            archiveRepo.Save();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete($"{ApplicationPath.Archives}\\{Settings.CurrentTestGalaxy}.db");
        }

        [Test]
        public void HasValidInclusionOption_IsConfigured_ReturnsTrue()
        {
            var validator = new ArchiveSettingValidator(Settings.CurrentTestGalaxy);
            
            using var objectRepo = new ObjectRepository(Settings.CurrentTestGalaxy);
            var gObject = objectRepo.FindByTagName("$Site_Data");

            var result = validator.HasValidInclusionOption(gObject.ObjectId);
            
            Assert.True(result);
        }
        
        [Test]
        public void HasValidInclusionOption_NotConfigured_ReturnsTrue()
        {
            var validator = new ArchiveSettingValidator(Settings.CurrentTestGalaxy);
            
            using var objectRepo = new ObjectRepository(Settings.CurrentTestGalaxy);
            var gObject = objectRepo.FindByTagName("$InTouchViewApp");

            var result = validator.HasValidInclusionOption(gObject.ObjectId);
            
            Assert.False(result);
        }
        
        [Test]
        public void IsValidArchiveTrigger_IsConfigured_ReturnsTrue()
        {
            var validator = new ArchiveSettingValidator(Settings.CurrentTestGalaxy);
            
            var result = validator.IsValidArchiveTrigger(0);
            
            Assert.True(result);
        }
        
        [Test]
        public void IsValidArchiveTrigger_NotConfigured_ReturnsFalse()
        {
            var validator = new ArchiveSettingValidator(Settings.CurrentTestGalaxy);
            
            var result = validator.IsValidArchiveTrigger(13);
            
            Assert.False(result);
        }
    }
}