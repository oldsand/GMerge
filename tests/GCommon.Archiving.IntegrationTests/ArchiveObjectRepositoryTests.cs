using System.IO;
using System.Linq;
using System.Text;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Extensions;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GCommon.Archiving;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class ArchiveObjectRepositoryTests
    {
        private SqliteConnectionStringBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SqliteConnectionStringBuilder {DataSource = @"\TestArchive.db"};

            var config = ArchiveConfiguration
                .Default("TestArchive")
                .OverrideConnectionString(_builder.ConnectionString);
            
            var archiveBuilder = new ArchiveBuilder();
            archiveBuilder.Build(config);
        }

        [TearDown]
        public void TearDown()
        {
            var fileName = _builder.DataSource;
            File.Delete(fileName);
        }

        [Test]
        public void Exists_ObjectExists_ReturnsTrue()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Objects.Exists(1);

            Assert.True(result);
        }

        [Test]
        public void Exists_ObjectDoesNotExist_ReturnsFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Objects.Exists(21);

            Assert.False(result);
        }

        [Test]
        public void Get_ExistingObjectId_ReturnsExpectedObject()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

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
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Objects.Get(100);

            Assert.Null(result);
        }

        [Test]
        public void Get_ExistingObjectIdWithEntries_IncludesEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Objects.Get(311);

            Assert.That(result.Entries, Has.Count.EqualTo(1));

            var entry = result.Entries.Single();
            Assert.NotNull(entry);
            Assert.AreEqual("This is some data to convert to binary",
                Encoding.UTF8.GetString(entry.CompressedData.Decompress()));
        }

        [Test]
        public void Get_ExistingObjectTagName_IncludesEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.Objects.FindByTagName("Tag1").ToList();

            Assert.That(results, Has.Count.EqualTo(2));
        }

        [Test]
        public void Get_NonExistingObjectTagName_ReturnsEmptyCollection()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.Objects.FindByTagName("Tag4").ToList();

            Assert.IsEmpty(results);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsExpectedObjects()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

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
            using var repo = new ArchiveRepository(_builder.ConnectionString);
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
            using var repo = new ArchiveRepository(_builder.ConnectionString);
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
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(1, "Some Test Object", 2, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"));

            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(1);

            Assert.That(target.Entries, Has.Count.EqualTo(1));

            var entry = target.Entries.First();
            Assert.NotNull(entry);

            var data = Encoding.UTF8.GetString(entry.CompressedData.Decompress());
            Assert.AreEqual("This is a new entry test", data);
        }

        [Test]
        public void Upsert_ExistingObjectWithEntryAddEntry_ReturnsExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(311, "Upsert Tester", 123, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"));

            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(311);

            Assert.That(target.Entries, Has.Count.EqualTo(2));

            var latest = target.GetLatestEntry();
            Assert.NotNull(latest);
            Assert.AreEqual("This is a new entry test", Encoding.UTF8.GetString(latest.CompressedData.Decompress()));
        }

        [Test]
        public void Upsert_NewObjectAddEntry_ReturnsExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(12, "New Object", 1, Template.Symbol);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is a new entry test"));

            repo.Objects.Upsert(archiveObject);
            repo.Save();

            var target = repo.Objects.Get(12);

            Assert.That(target.Entries, Has.Count.EqualTo(1));

            var entry = target.Entries.First();
            Assert.NotNull(entry);

            var data = Encoding.UTF8.GetString(entry.CompressedData.Decompress());
            Assert.AreEqual("This is a new entry test", data);
        }

        [Test]
        public void Delete_ExistingObjet_ObjectExistsReturnFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Objects.Delete(2);
            repo.Save();

            var result = repo.Objects.Exists(2);
            Assert.False(result);
        }

        [Test]
        public void Delete_NonExistingObjet_ObjectExistsReturnFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Objects.Delete(6);
            repo.Save();

            var result = repo.Objects.Exists(6);
            Assert.False(result);
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_builder.ConnectionString).Options;
            using var context = new ArchiveContext(options);

            context.Objects.Add(new ArchiveObject(1, "Tag1", 21, Template.UserDefined));
            context.Objects.Add(new ArchiveObject(2, "Tag2", 33, Template.Area));
            context.Objects.Add(new ArchiveObject(3, "Tag3", 13, Template.Symbol));
            context.Objects.Add(new ArchiveObject(4, "Tag1", 2, Template.ViewEngine));

            var archiveObject = new ArchiveObject(311, "TestObject", 2, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is some data to convert to binary"));
            context.Objects.Add(archiveObject);

            context.SaveChanges();
        }
    }
}