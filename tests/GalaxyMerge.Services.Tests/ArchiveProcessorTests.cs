using System.IO;
using GalaxyMerge.Archestra;
using GalaxyMerge.Archive;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Testing;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ArchiveProcessorTests
    {
        [SetUp]
        public void Setup()
        {
            var builder = new ArchiveBuilder();
            builder.Build(ArchiveConfigurationBuilder.Default(Settings.CurrentTestGalaxy));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete($"{ApplicationPath.Archives}\\{Settings.CurrentTestGalaxy}.db");
        }
        
        [Test]
        [TestCase("$Test_Template")]
        [TestCase("$Site_Data")]
        [TestCase("FileCopy")]
        [TestCase("DatabaseAccess")]
        public void Archive_ValidTagName_CreateArchiveEntry(string tagName)
        {
            var galaxyRepo = new GalaxyRepository(Settings.CurrentTestGalaxy);
            galaxyRepo.Login("");
            
            var archiver = new ArchiveProcessor(galaxyRepo);

            using var objectRepo = new ObjectRepository(Settings.CurrentTestGalaxy);
            var gObject = objectRepo.FindByTagName(tagName);

            archiver.Archive(gObject.ObjectId);

            using var archiveRepo = new ArchiveRepository(Settings.CurrentTestGalaxy);
            var result = archiveRepo.GetObjectIncludeEntries(gObject.ObjectId);
            
            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.IsNotEmpty(result.Entries);
        }
    }
}