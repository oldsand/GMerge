using System.IO;
using System.Linq;
using System.Xml.Linq;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core.Extensions;
using NUnit.Framework;

namespace GalaxyMerge.Archive.Tests
{
    [TestFixture]
    public class ArchiveRepositoryTests
    {
        private ArchiveConfigurationBuilder _configBuilder;
        private const string GalaxyName = "Sandbox";

        [SetUp]
        public void Setup()
        {
            _configBuilder = ArchiveConfigurationBuilder.Default(GalaxyName, 4, "123", "456");
            var builder = new ArchiveBuilder();
            builder.Build(_configBuilder);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_configBuilder.FileName);
        }

        [Test]
        public void Constructor_WhenCalled_ReturnsNotNull()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            Assert.NotNull(repo);
        }
        
        [Test]
        public void AddObject_WhenCalled_AddObjectAndCanGetThatObject()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            
            var archiveObject = new ArchiveObject(1234, "$Some_Tag_Name", 34, Template.UserDefined);
            
            repo.AddObject(archiveObject);
            repo.Save();

            var result = repo.GetObject(archiveObject.ObjectId);
            Assert.NotNull(result);
            Assert.AreEqual(1234, result.ObjectId);
            Assert.AreEqual("$Some_Tag_Name", result.TagName);
            Assert.AreEqual(34, result.Version);
            Assert.AreEqual(Template.UserDefined, result.Template);
        }

        [Test]
        public void AddObject_WithEntry_ReturnsExpectedResults()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            var archiveObject = new ArchiveObject(15649, "$TagName", 123, Template.Symbol);
            var data = new XElement("SomeObject", "SomeData").ToByteArray();
            archiveObject.AddEntry(data);

            repo.AddObject(archiveObject);
            repo.Save();
            var result = repo.GetObject(archiveObject.ObjectId);
            
            Assert.NotNull(result);
            Assert.AreEqual(15649, result.ObjectId);
            Assert.AreEqual("$TagName", result.TagName);
            Assert.AreEqual(123, result.Version);
            Assert.AreEqual(Template.Symbol, result.Template);
            Assert.IsNotEmpty(result.Entries);
        }

        /*[Test]
        public void GetLatest_WhenCalled_ReturnsSingleEntry()
        {
            using var repo = new ArchiveRepository(GalaxyName);

            var result = repo.GetLatestEntry("SomeTag");
            
            Assert.NotNull(result);
        }

        [Test]
        public void FindByObjectId_WhenCalled_ReturnsEntryWithObjectId()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            
            var data = new XElement("SomeObject", "SomeData");
            var entry = new ArchiveEntry(53262, "TagName", 4, "$Area", data.ToByteArray());
            repo.AddEntry(entry);
            repo.Save();

            var results = repo.FindEntriesByObjectId(53262).ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.Any(x => x.ObjectId == 53262));
        }

        [Test]
        public void FindByTagName_WhenCalled_ReturnsObjectsWithTagName()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            
            var data = new XElement("SomeObject", "SomeData");
            var entry = new ArchiveEntry(1234, "Single_Tag_Name", 4, "$Area", data.ToByteArray());
            repo.AddEntry(entry);
            repo.Save();

            var results = repo.FindByTagName("Single_Tag_Name").ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.Any(x => x.TagName == "Single_Tag_Name"));

        }*/
    }
}