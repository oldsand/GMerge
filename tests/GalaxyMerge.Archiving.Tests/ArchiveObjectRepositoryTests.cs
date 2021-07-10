using System.IO;
using System.Linq;
using System.Text;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Extensions;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GCommon.Archiving;
using Microsoft.EntityFrameworkCore; 
using NUnit.Framework;

namespace GalaxyMerge.Archiving.Tests
{
    [TestFixture]
    public class ArchiveObjectRepositoryTests
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
        public void Exists_ObjectExists_ReturnsTrue()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Exists(1);
            
            Assert.True(result);
        }
        
        [Test]
        public void Exists_ObjectDoesNotExist_ReturnsFalse() 
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Exists(21);
            
            Assert.False(result);
        }
        
        [Test]
        public void Get_ExistingObjectId_ReturnsExpectedObject()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Get(1);
            
            Assert.NotNull(result);
            Assert.AreEqual("Tag1", result.TagName);
            Assert.AreEqual(21, result.Version);
            Assert.AreEqual(Template.UserDefined, result.Template);
        }
        
        [Test]
        public void Get_NonExistingObjectId_ReturnsNull()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Get(100);
            
            Assert.Null(result);
        }
        
        [Test]
        public void Get_ExistingObjectIdWithEntries_IncludesEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Objects.Get(311);
            
            Assert.That(result.Entries, Has.Count.EqualTo(1));
        }
        
        [Test]
        public void Get_ExistingObjectTagName_IncludesEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var results = repo.Objects.FindByTagName("Tag1").ToList();
            
            Assert.That(results, Has.Count.EqualTo(2));
        }
        
        [Test]
        public void Get_NonExistingObjectTagName_ReturnsEmptyCollection()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var results = repo.Objects.FindByTagName("Tag4").ToList();
            
            Assert.IsEmpty(results);
        }
        
        [Test]
        public void GetAll_WhenCalled_ReturnsExpectedObjects()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);

            var all = repo.Objects.GetAll().ToList();
            
            Assert.That(all.Select(x => x.TagName), Contains.Item("Tag1"));
            Assert.That(all.Select(x => x.TagName), Contains.Item("Tag2"));
            Assert.That(all.Select(x => x.TagName), Contains.Item("Tag3"));
            Assert.That(all.Select(x => x.TagName), Contains.Item("TestObject"));
        }

        [Test]
        public void Upsert_ExistingObject_ReturnsExpectedObject()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            var archiveObject = new ArchiveObject(1, "Some Test Object", 2, Template.UserDefined);
            
            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(1);
            
            Assert.NotNull(target);
            Assert.AreEqual("Some Test Object", target.TagName);
            Assert.AreEqual(2, target.Version);
        }
        
        [Test]
        public void Upsert_NewObject_ReturnsExpectedObject()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            var archiveObject = new ArchiveObject(12, "New Object", 1, Template.Symbol);
            
            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(12);

            Assert.NotNull(target);
            Assert.AreEqual("New Object", target.TagName);
            Assert.AreEqual(1, target.Version);
            Assert.AreEqual(Template.Symbol, target.Template);
        }

        [Test]
        public void Upsert_ExistingObjectAddEntry_ReturnsExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            var archiveObject = new ArchiveObject(1, "Some Test Object", 2, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"), 432156);
            
            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(1);
            
            Assert.That(target.Entries, Has.Count.EqualTo(1));
            
            var entry = target.Entries.First();
            Assert.NotNull(entry);
            Assert.AreEqual(432156, entry.ChangeLogId);
            
            var data = Encoding.UTF8.GetString(entry.CompressedData.Decompress());
            Assert.AreEqual("This is a new entry test", data);
        }
        
        [Test]
        public void Upsert_ExistingObjectWithEntryAddEntry_ReturnsExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            var archiveObject = new ArchiveObject(311, "Upsert Tester", 123, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"), 432156);
            
            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(311);
            
            Assert.That(target.Entries, Has.Count.EqualTo(2));
            
            var first = target.Entries.Single(x => x.ChangeLogId == 432156);
            Assert.NotNull(first);
            Assert.AreEqual(432156, first.ChangeLogId);
            Assert.AreEqual("This is a new entry test", Encoding.UTF8.GetString(first.CompressedData.Decompress()));
            
            var second = target.Entries.Single(x => x.ChangeLogId == 34523);
            Assert.NotNull(second);
            Assert.AreEqual(34523, second.ChangeLogId);
            Assert.AreEqual("This is some data to convert to binary", 
                Encoding.UTF8.GetString(second.CompressedData.Decompress()));
        }
        
        [Test]
        public void Upsert_NewObjectAddEntry_ReturnsExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            var archiveObject = new ArchiveObject(12, "New Object", 1, Template.Symbol);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"), 432156);
            
            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(12);
            
            Assert.That(target.Entries, Has.Count.EqualTo(1));
            
            var entry = target.Entries.First();
            Assert.NotNull(entry);
            Assert.AreEqual(432156, entry.ChangeLogId);
            
            var data = Encoding.UTF8.GetString(entry.CompressedData.Decompress());
            Assert.AreEqual("This is a new entry test", data);
        }

        [Test]
        public void Delete_ExistingObjet_ObjectExistsReturnFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            
            repo.Objects.Delete(2);
            repo.Save();

            var result = repo.Objects.Exists(2);
            Assert.False(result);
        }
        
        [Test]
        public void Delete_NonExistingObjet_ObjectExistsReturnFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_connectionString);
            
            repo.Objects.Delete(6);
            repo.Save();

            var result = repo.Objects.Exists(6);
            Assert.False(result);
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_connectionString).Options;
            using var context = new ArchiveContext(options);

            context.Objects.Add(new ArchiveObject(1, "Tag1", 21, Template.UserDefined));
            context.Objects.Add(new ArchiveObject(2, "Tag2", 33, Template.Area));
            context.Objects.Add(new ArchiveObject(3, "Tag3", 13, Template.Symbol));
            context.Objects.Add(new ArchiveObject(4, "Tag1", 2, Template.ViewEngine));

            var archiveObject = new ArchiveObject(311, "TestObject", 2, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is some data to convert to binary"), 34523);
            context.Objects.Add(archiveObject);

            context.SaveChanges();
        }
    }
}