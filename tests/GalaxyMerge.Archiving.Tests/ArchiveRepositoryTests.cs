using System;
using System.IO;
using System.Linq;
using System.Text;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Archiving.Repositories;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GalaxyMerge.Archiving.Tests
{
    internal class ArchiveRepositoryTests
    {
        private const string GalaxyName = "GalaxyName";
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var config = ArchiveConfiguration.Default(GalaxyName);
            var builder = new ArchiveBuilder();
            builder.Build(config);
            
            _connectionString = DbStringBuilder.ArchiveString(GalaxyName);
        }
        
        [TearDown]
        public void TearDown()
        {
            var fileName = Path.Combine(ApplicationPath.Archives, $"{GalaxyName}.db");
            File.Delete(fileName);
        }

        [Test]
        public void GetArchiveInfo_WhenCalled_ReturnsNotNull()
        {
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchiveInfo();
            
            Assert.NotNull(archive);
        }
        
        [Test]
        public void GetArchiveInfo_WhenCalled_ReturnsExpectedInfo()
        {
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchiveInfo();
            
            Assert.AreEqual(1, archive.ArchiveId);
            Assert.AreEqual(GalaxyName, archive.GalaxyName);
            Assert.AreEqual(ArchestraVersion.Sp2012R2P03, archive.Version);
            Assert.That(archive.CreatedOn, Is.EqualTo(DateTime.Now).Within(5).Seconds);
            Assert.That(archive.UpdatedOn, Is.EqualTo(DateTime.Now).Within(5).Seconds);
        }
        
        [Test]
        public void GetArchiveSettings_WhenCalled_ReturnsNotEmptySettings()
        {
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchiveSettings();
            
            Assert.NotNull(archive.EventSettings);
            Assert.NotNull(archive.InclusionSettings);
            Assert.NotNull(archive.IgnoreSettings);
            
            Assert.IsNotEmpty(archive.EventSettings);
            Assert.IsNotEmpty(archive.InclusionSettings);
        }
        
        [Test]
        public void GetArchive_WhenCalled_ReturnsNotEmptyObjects()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchive();
            
            Assert.NotNull(archive.Objects);
            Assert.IsNotEmpty(archive.Objects);
        }
        
        [Test]
        public void GetArchive_WhenCalled_ReturnsExpectedObjects()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchive();
            
            Assert.That(archive.Objects.Select(x => x.TagName), Contains.Item("Tag1"));
            Assert.That(archive.Objects.Select(x => x.TagName), Contains.Item("Tag2"));
            Assert.That(archive.Objects.Select(x => x.TagName), Contains.Item("Tag3"));
            Assert.That(archive.Objects.Select(x => x.TagName), Contains.Item("TestObject"));
        }

        [Test]
        public void GetArchive_FindObject_ReturnsNotNull()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchive();
            var target = archive.Objects.Single(x => x.TagName == "TestObject");
            
            Assert.NotNull(target);
        }
        
        
        [Test]
        public void GetArchive_FindObject_ReturnsEmptyEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchive();
            var target = archive.Objects.Single(x => x.TagName == "TestObject");
            
            Assert.IsEmpty(target.Entries);
        }
        
        [Test]
        public void UpdateObject_ExistingObjectWithEntries_ReturnsExpectedObjectAndEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchive();
            
            var archiveObject = new ArchiveObject(311, "TestObject", 3, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"), 432156);
            archive.UpdateObject(archiveObject);

            repo.Save();

            var target = repo.Objects.Get(311);
            
            Assert.NotNull(target);
            Assert.AreEqual("TestObject", target.TagName);
            Assert.AreEqual(3, target.Version);
            Assert.IsNotEmpty(target.Entries);
            Assert.That(target.Entries, Has.Count.EqualTo(2));
        }
        
        [Test]
        public void UpdateObject_NewObjectWithEntries_ReturnsExpectedObjectAndEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var archive = repo.GetArchive();
            
            var archiveObject = new ArchiveObject(100, "NewObject", 12, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry on a new object test"), 432156);
            archive.UpdateObject(archiveObject);
            
            repo.Save();

            var target = repo.Objects.Get(311);
            var data = Encoding.UTF8.GetString(target.Entries.First().CompressedData);
            
            Assert.NotNull(target);
            Assert.AreEqual("TestObject", target.TagName);
            Assert.AreEqual(1, target.Version);
            Assert.IsNotEmpty(target.Entries);
            Assert.That(target.Entries, Has.Count.EqualTo(1));
            Assert.AreEqual("This is a new entry on a new object test", data);
        }

        [Test]
        public void ObjectsGet_WhenCalled_ReturnsObjectNotNull()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Get(311);

            Assert.NotNull(result);
        }
        
        [Test]
        public void ObjectsGet_WhenCalled_ReturnsEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Get(311);

            Assert.NotNull(result.Entries);
            Assert.IsNotEmpty(result.Entries);
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_connectionString).Options;
            using var context = new ArchiveContext(options);

            context.Objects.Add(new ArchiveObject(1, "Tag1", 21, Template.UserDefined){ArchiveId = 1});
            context.Objects.Add(new ArchiveObject(2, "Tag2", 33, Template.Area){ArchiveId = 1});
            context.Objects.Add(new ArchiveObject(3, "Tag3", 13, Template.Symbol){ArchiveId = 1});

            var archiveObject = new ArchiveObject(311, "TestObject", 2, Template.UserDefined){ArchiveId = 1};
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is some data to convert to binary"), 34523);
            context.Objects.Add(archiveObject);

            context.Queue.Add(new QueuedEntry(132432, 2, 0, DateTime.Today));
            context.Queue.Add(new QueuedEntry(123523, 311, 4, DateTime.Now));
            context.Queue.Add(new QueuedEntry(577854, 3, 14, DateTime.Today.AddHours(1)));

            context.SaveChanges();
        }
    }
}