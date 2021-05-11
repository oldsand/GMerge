using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
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
        public void GetGalaxyInfo_WhenCalled_ReturnsExpectedData()
        {
            using var repo = new ArchiveRepository(GalaxyName);

            var result = repo.GetGalaxyInfo();
            
            Assert.AreEqual(GalaxyName, result.GalaxyName);
            Assert.AreEqual(4, result.VersionNumber);
            Assert.AreEqual("456", result.IsaVersion);
            Assert.AreEqual("123", result.CdiVersion);
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

        [Test]
        public void GetLatest_WhenCalled_ReturnsSingleEntry()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            SeedArchiveObjects();

            var result = repo.GetLatestEntry(4);
            
            Assert.NotNull(result);
            Assert.AreEqual(4, result.ObjectId);
            Assert.NotNull(result.CompressedData);
        }

        [Test]
        public void GetObject_WhenCalled_ReturnsExpectedObject()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            SeedArchiveObjects();

            var result = repo.GetObject(3);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.ObjectId);
        }
        
        [Test]
        public void GetObjectIncludeEntries_WhenCalled_ReturnsExpectedObject()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            SeedArchiveObjects();

            var result = repo.GetObjectIncludeEntries(3);

            Assert.NotNull(result);
            Assert.IsNotEmpty(result.Entries);
            Assert.AreEqual(3, result.ObjectId);
        }

        [Test]
        public void FindAllByTagName_WhenCalled_ReturnsExpectedObjects()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            SeedArchiveObjects();

            var results = repo.FindAllByTagName("TagName_2").ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.Any(x => x.TagName == "TagName_2"));

        }

        private void SeedArchiveObjects()
        {
            using var context = new ArchiveContext(_configBuilder.ContextOptions);

            for (var i = 1; i < 10; i++)
            {
                var random = new Random();
                var template = Enumeration.FromId<Template>(random.Next(1, 19));
                var archiveObject = new ArchiveObject(i, $"TagName_{i}", i * 10, template);

                for (var j = 0; j < 5; j++)
                    archiveObject.AddEntry(new byte[10000]);
                
                context.ArchiveObjects.Add(archiveObject);
            }

            context.SaveChanges();
        }
    }
}