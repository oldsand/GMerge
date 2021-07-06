using System.IO;
using GalaxyMerge.Archiving;
using GalaxyMerge.Core.Utilities;
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
            builder.Build(ArchiveConfiguration.Default(Settings.CurrentTestGalaxy));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete($"{ApplicationPath.Archives}\\{Settings.CurrentTestGalaxy}.db");
        }
        
        /*[Test]
        [TestCase("$Test_Template")]
        [TestCase("$Site_Data")]
        [TestCase("FileCopy")]
        [TestCase("DatabaseAccess")]
        public void Archive_ValidTagName_CreateArchiveEntry(string tagName)
        {
            var galaxyRepo = new GalaxyRepository(Settings.CurrentTestGalaxy);
            galaxyRepo.Login("");
            
            using var dataRepo = new DataRepository(Settings.CurrentTestHost, Settings.CurrentTestGalaxy);
            
            var archiver = new ArchiveProcessor(galaxyRepo, dataRepo);
            
            var gObject = dataRepo.Objects.FindByTagName(tagName);

            archiver.Archive(gObject.ObjectId);

            using var archiveRepo = new ArchiveRepository(Settings.CurrentTestGalaxy);
            var result = archiveRepo.GetObjectIncludeEntries(gObject.ObjectId);
            
            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.IsNotEmpty(result.Entries);
        }*/
    }
}