using System;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using GCommon.Core.Extensions;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    internal class ArchiveRepositoryTests
    {
        private SqliteConnectionStringBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SqliteConnectionStringBuilder {DataSource = @"\TestArchive.db"};

            var archive = new ArchiveConfig("TestArchive");
            
            var archiveBuilder = new ArchiveBuilder();
            archiveBuilder.Build(archive, _builder.ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            var fileName = _builder.DataSource;
            File.Delete(fileName);
        }

        [Test]
        public void GetConfig_WhenCalled_ShouldNotBeNull()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var archive = repo.GetConfig();

            archive.Should().NotBeNull();
        }
        
        [Test]
        public void GetConfig_WhenCalled_ReturnsExpectedProperties()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var archive = repo.GetConfig();

            archive.ArchiveId.Should().Be(1);
            archive.GalaxyName.Should().Be("TestArchive");
            archive.Version.Should().Be(ArchestraVersion.SystemPlatform2012R2P3);
            archive.CreatedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            archive.UpdatedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            archive.EventSettings.Should().NotBeEmpty();
            archive.InclusionSettings.Should().NotBeNull();
        }

        [Test]
        public void IsArchivable_ValidArchivableObject_ShouldReturnTrue()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(1, "$SomeTag", 1, Template.UserDefined);
            
            var result = repo.IsArchivable(archiveObject, Operation.CreateInstance);

            result.Should().BeTrue();
        }

        [Test]
        public void ObjectExists_ObjectExists_ReturnsTrue()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.ObjectExists(1);

            Assert.True(result);
        }

        [Test]
        public void ObjectExists_ObjectDoesNotExist_ReturnsFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.ObjectExists(21);

            Assert.False(result);
        }

        [Test]
        public void GetObject_ExistingObjectId_ReturnsExpectedObject()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetObject(1);

            Assert.NotNull(result);
            Assert.AreEqual("Tag1", result.TagName);
            Assert.AreEqual(21, result.Version);
            Assert.AreEqual(Template.UserDefined, result.Template);
        }

        [Test]
        public void GetObject_NonExistingObjectId_ReturnsNull()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetObject(100);

            Assert.Null(result);
        }

        [Test]
        public void GetObject_ExistingObjectIdWithEntries_IncludesEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetObject(311);

            Assert.That(result.Entries, Has.Count.EqualTo(1));

            var entry = result.Entries.Single();
            Assert.NotNull(entry);
            Assert.AreEqual("This is some data to convert to binary",
                Encoding.UTF8.GetString(entry.CompressedData.Decompress()));
        }
        
        [Test]
        public void GetObject_ObjectWithLog_LogsShouldBeExpectedValues()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetObject(311);
            result.Logs.Should().NotBeEmpty();

            var log = result.Logs.First();
            log.Should().NotBeNull();
            log.ChangeLogId.Should().Be(14);
            log.ObjectId.Should().Be(311);
            log.ChangedOn.Should().Be(DateTime.Today);
            log.Operation.Should().Be(Operation.CreateInstance);
        }

        [Test]
        public void GetObjects_ExistingObjectTagName_IncludesEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.GetObjects("Tag1").ToList();

            Assert.That(results, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetObjects_NonExistingObjectTagName_ReturnsEmptyCollection()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.GetObjects("Tag4").ToList();

            Assert.IsEmpty(results);
        }

        [Test]
        public void GetObjects_WhenCalled_ReturnsExpectedObjects()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var all = repo.GetObjects().ToList();

            Assert.That(all.Select(x => x.TagName), Contains.Item("Tag1"));
            Assert.That(all.Select(x => x.TagName), Contains.Item("Tag2"));
            Assert.That(all.Select(x => x.TagName), Contains.Item("Tag3"));
            Assert.That(all.Select(x => x.TagName), Contains.Item("TestObject"));
        }

        [Test]
        public void UpsertObject_ExistingObject_ShouldReturnExpectedProperties()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(1, "Some Test Object", 2, Template.UserDefined);

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(1);

            target.TagName.Should().Be("Some Test Object");
            target.Version.Should().Be(2);
            target.Template.Should().Be(Template.UserDefined);
            target.IsTemplate.Should().BeFalse();
            target.ModifiedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
        }

        [Test]
        public void UpsertObject_NewObject_ShouldReturnExpectedProperties()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(12, "New Object", 1, Template.Symbol);

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(12);

            target.Should().NotBeNull();
            target.TagName.Should().Be("New Object");
            target.Version.Should().Be(1);
            target.Template.Should().Be(Template.Symbol);
            target.IsTemplate.Should().BeFalse();
            target.ModifiedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            target.AddedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
        }

        [Test]
        public void UpsertObject_ExistingObjectWithAddedEntry_ShouldReturnExpectedEntry()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(2, "Some Test Object", 2, Template.UserDefined);
            archiveObject.Archive(Encoding.UTF8.GetBytes("This is a new entry test"));

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(2);
            target.Entries.Should().HaveCount(1);

            var entry = target.Entries.First();
            entry.Should().NotBeNull();
            entry.Version.Should().Be(2);
            entry.ArchivedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            entry.ObjectId.Should().Be(2);

            var data = Encoding.UTF8.GetString(entry.CompressedData.Decompress());
            data.Should().Be("This is a new entry test");
        }
        
         [Test]
        public void UpsertObject_ExistingObjectWithAddedLog_ShouldReturnExpectedLog()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(2, "Some Test Object", 2, Template.UserDefined);
            archiveObject.Archive(Encoding.UTF8.GetBytes("We need to add an entry to add a log to"));
            archiveObject.GetLatestEntry().UpdateLog(213, DateTime.Now, Operation.Rename, "Comment", Environment.UserName);

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(2);
            target.Logs.Should().HaveCount(1);

            var log = target.Logs.First();
            
            log.Should().NotBeNull();
            log.ChangeLogId.Should().Be(213);
            log.ChangedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            log.Operation.Should().Be(Operation.Rename);
            log.Comment.Should().Be("Comment");
            log.UserName.Should().Be(Environment.UserName);
        }

        [Test]
        public void UpsertObject_ExistingObjectWithEntryAndAddedEntry_ShouldReturnExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(311, "Upsert Tester", 123, Template.UserDefined);
            archiveObject.Archive(Encoding.UTF8.GetBytes("This is a new entry test"));

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(311);
            target.Entries.Should().HaveCount(2);

            var latest = target.GetLatestEntry();
            latest.Should().NotBeNull();
            latest.Version.Should().Be(123);
            latest.ArchivedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            latest.ObjectId.Should().Be(311);

            var data = Encoding.UTF8.GetString(latest.CompressedData.Decompress());
            data.Should().Be("This is a new entry test");
        }

        [Test]
        public void UpsertObject_ExistingObjectWithLogAndAddedLog_ShouldReturnExpectedLogs()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(311, "Upsert Tester", 123, Template.UserDefined);
            archiveObject.Archive(Encoding.UTF8.GetBytes("We need to add a new  entry to add a new log to"));
            archiveObject.GetLatestEntry().UpdateLog(213, DateTime.Now, Operation.Rename, "Comment", Environment.UserName);

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(311);
            target.Logs.Should().HaveCount(2);

            var latest = target.GetLatestLog();
            latest.Should().NotBeNull();
            latest.ChangeLogId.Should().Be(213);
            latest.ChangedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            latest.Operation.Should().Be(Operation.Rename);
            latest.Comment.Should().Be("Comment");
            latest.UserName.Should().Be(Environment.UserName);
        }

        [Test]
        public void UpsertObject_NewObjectAddEntry_ReturnsExpectedEntries()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(12, "New Object", 1, Template.Symbol);
            archiveObject.Archive(Encoding.UTF8.GetBytes("This is a new entry test"));

            repo.UpsertObject(archiveObject);
            repo.Save();

            var target = repo.GetObject(12);
            target.Entries.Should().HaveCount(1);

            var entry = target.Entries.First();
            entry.Should().NotBeNull();
            entry.Version.Should().Be(1);
            entry.ArchivedOn.Should().BeWithin(TimeSpan.FromSeconds(1));
            entry.ObjectId.Should().Be(12);

            var data = Encoding.UTF8.GetString(entry.CompressedData.Decompress());
            data.Should().Be("This is a new entry test");
        }

        [Test]
        public void DeleteObject_ExistingObjet_ObjectExistsReturnFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.DeleteObject(2);
            repo.Save();

            var result = repo.ObjectExists(2);
            result.Should().BeFalse();
        }

        [Test]
        public void DeleteObject_NonExistingObjet_ObjectExistsReturnFalse()
        {
            Seed();
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.DeleteObject(6);
            repo.Save();

            var result = repo.ObjectExists(6);
            result.Should().BeFalse();
        }
        
        [Test]
        public void GetLog_ExistingLogShouldReturnExpectedProperties()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetLog(1);

            result.Should().NotBeNull();
            result.ArchiveObject.Should().NotBeNull();
            result.ChangeLogId.Should().Be(1);
            result.ChangedOn.Should().Be(DateTime.Today);
            result.Operation.Should().Be(Operation.CreateInstance);
            result.Comment.Should().Be("Created new instance");
            result.UserName.Should().Be(Environment.UserName);
        }

        [Test]
        public void GetLog_NonExistingLog_ShouldBeNull()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetLog(10);

            result.Should().BeNull();
        }

        [Test]
        public void GetLogs_WhenCalled_ShouldHaveExpectedCount()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.GetLogs().ToList();

            results.Should().HaveCount(5);
        }

        [Test]
        public void GetLogs_EmptyDatabase_ShouldBeEmpty()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.GetLogs().ToList();

            results.Should().BeEmpty();
        }

        [Test]
        public void FindLogs_ByOperation_ReturnsExpectedEntity()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.FindLogs(x => x.Operation == Operation.CreateInstance).ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.All(x => x.Operation == Operation.CreateInstance));
        }

        [Test]
        public void FindLogs_ByDateTime_ReturnsExpectedEntities()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.FindLogs(x => x.ChangedOn == DateTime.Today).ToList();

            results.Should().HaveCount(2);
        }
        
        [Test]
        public void GetQueuedLog_ExistingLog_ReturnsExpected()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetQueuedLog(2);

            result.Should().NotBeNull();
            result.ChangeLogId.Should().Be(2);
        }

        [Test]
        public void GetQueuedLog_NonExistingLog_ReturnsNull()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.GetQueuedLog(2221);

            result.Should().BeNull();
        }

        [Test]
        public void Enqueue_NonExistingId_AddsEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var log = new QueuedLog(6, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username");
            repo.Enqueue(log);
            repo.Save();

            var result = repo.GetQueuedLog(6);

            result.Should().NotBeNull();
            result.ChangeLogId.Should().Be(6);
            result.ObjectId.Should().Be(321);
            result.Operation.Should().Be(Operation.CheckInSuccess);
            result.Comment.Should().Be("Comment");
            result.UserName.Should().Be("username");
        }

        [Test]
        public void Enqueue_ExistingId_ShouldBeAbleToGetEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var log = new QueuedLog(2, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username");
            repo.Enqueue(log);
            repo.Save();

            var result = repo.GetQueuedLog(2);

            result.Should().NotBeNull();
            result.ChangeLogId.Should().Be(2);
        }

        [Test]
        public void Dequeue_ExistingId_RemovesEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Dequeue(3);
            repo.Save();

            var result = repo.GetQueuedLog(3);

            result.Should().BeNull();
        }

        [Test]
        public void Dequeue_NonExistingId_ReturnsNull()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Dequeue(300);
            repo.Save();

            var result = repo.GetQueuedLog(300);

            result.Should().BeNull();
        }
        
        [Test]
        public void UpdateState_ExistingLog_ReturnsUpdatedEntity()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);
            var target = repo.GetQueuedLog(2);
            target.State.Should().Be(ArchiveState.New);
            
            target.State = ArchiveState.Processing;
            repo.Save();
            
            target = repo.GetQueuedLog(2);
            target.State.Should().Be(ArchiveState.Processing);
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_builder.ConnectionString).Options;
            using var context = new ArchiveContext(options);
            
            var obj = new ArchiveObject(1, "Tag1", 21, Template.UserDefined);
            context.Objects.Add(obj);

            context.Objects.Add(obj);
            context.Objects.Add(new ArchiveObject(2, "Tag2", 33, Template.Area));
            context.Objects.Add(new ArchiveObject(3, "Tag3", 13, Template.Symbol));
            context.Objects.Add(new ArchiveObject(4, "Tag1", 2, Template.ViewEngine));

            var aObjectWithEntryAndLog = new ArchiveObject(311, "TestObject", 2, Template.UserDefined);
            aObjectWithEntryAndLog.Archive(Encoding.UTF8.GetBytes("This is some data to convert to binary"));
            var entry = aObjectWithEntryAndLog.GetLatestEntry();
            entry.UpdateLog(14, DateTime.Today, Operation.CreateInstance, "Created object", "username");
            context.Objects.Add(aObjectWithEntryAndLog);

            context.Entries.Add(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Entry Data 1")));
            context.Entries.Add(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Entry Data 2")));
            context.Entries.Add(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Entry Data 3")));

            context.Logs.Add(new ArchiveLog(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test1")), 1, DateTime.Today,
                Operation.CreateInstance, "Created new instance", Environment.UserName));
            context.Logs.Add(new ArchiveLog(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test2")), 2,
                DateTime.Today.AddHours(2), Operation.CheckInSuccess, "Updated current object", Environment.UserName));
            context.Logs.Add(new ArchiveLog(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test3")), 3,
                DateTime.Today.AddHours(3), Operation.Rename, "Renamed instance", Environment.UserName));
            context.Logs.Add(new ArchiveLog(new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test4")), 4,
                DateTime.Today.AddHours(4), Operation.ModifiedAutomationObjectOnly, "Modified something",
                Environment.UserName));

            
            context.Queue.Add(new QueuedLog(1, 311, DateTime.Today, Operation.CreateInstance, "Comment", "username"));
            context.Queue.Add(new QueuedLog(2, 311, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));
            context.Queue.Add(new QueuedLog(3, 311, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));
            context.Queue.Add(new QueuedLog(4, 311, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));
            context.Queue.Add(new QueuedLog(5, 311, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));

            context.SaveChanges();
        }
    }
}