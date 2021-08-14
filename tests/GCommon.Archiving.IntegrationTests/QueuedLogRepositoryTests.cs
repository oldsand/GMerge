using System;
using System.IO;
using GCommon.Archiving.Repositories;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class QueuedLogRepositoryTests
    {
        private SqliteConnectionStringBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SqliteConnectionStringBuilder {DataSource = @"\TestArchive.db"};

            var archive = new Archive("TestArchive");

            var archiveBuilder = new ArchiveBuilder();
            archiveBuilder.Build(archive, _builder.ConnectionString);

            Seed();
        }

        [TearDown]
        public void TearDown()
        {
            var fileName = _builder.DataSource;
            File.Delete(fileName);
        }

        [Test]
        public void Get_ExistingLog_ReturnsExpected()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Queue.Get(2);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.ChangeLogId);
        }

        [Test]
        public void Get_NonExistingLog_ReturnsNull()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Queue.Get(2221);

            Assert.Null(result);
        }

        [Test]
        public void Enqueue_NonExistingId_AddsEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var log = new QueuedLog(6, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username");
            repo.Queue.Enqueue(log);
            repo.Save();

            var result = repo.Queue.Get(6);

            Assert.NotNull(result);
            Assert.AreEqual(6, result.ChangeLogId);
            Assert.AreEqual(321, result.ObjectId);
            Assert.AreEqual(Operation.CheckInSuccess, result.Operation);
            Assert.AreEqual("Comment", result.Comment);
            Assert.AreEqual("username", result.UserName);
        }

        [Test]
        public void Enqueue_ExistingId_AddsEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var log = new QueuedLog(2, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username");
            repo.Queue.Enqueue(log);
            repo.Save();

            var result = repo.Queue.Get(2);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.ChangeLogId);
        }

        [Test]
        public void Dequeue_ExistingId_RemovesEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Queue.Dequeue(3);
            repo.Save();

            var result = repo.Queue.Get(3);

            Assert.Null(result);
        }

        [Test]
        public void Dequeue_NonExistingId_ReturnsNull()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Queue.Dequeue(300);
            repo.Save();

            var result = repo.Queue.Get(300);

            Assert.Null(result);
        }
        
        [Test]
        public void UpdateState_ExistingLog_ReturnsUpdatedEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);
            var target = repo.Queue.Get(2);
            Assert.AreEqual(ArchiveState.New, target.State);
            
            target.State = ArchiveState.Processing;
            repo.Save();
            
            target = repo.Queue.Get(2);
            Assert.AreEqual(ArchiveState.Processing, target.State);
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_builder.ConnectionString).Options;
            using var context = new ArchiveContext(options);

            context.Queue.Add(new QueuedLog(1, 321, DateTime.Today, Operation.CreateInstance, "Comment", "username"));
            context.Queue.Add(new QueuedLog(2, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));
            context.Queue.Add(new QueuedLog(3, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));
            context.Queue.Add(new QueuedLog(4, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));
            context.Queue.Add(new QueuedLog(5, 321, DateTime.Now, Operation.CheckInSuccess, "Comment", "username"));

            context.SaveChanges();
        }
    }
}