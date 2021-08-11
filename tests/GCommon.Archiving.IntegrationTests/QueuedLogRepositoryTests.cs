using System.IO;
using GCommon.Archiving.Repositories;
using GCommon.Primitives;
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

            repo.Queue.Enqueue(6);
            repo.Save();

            var result = repo.Queue.Get(6);
        
            Assert.NotNull(result);
            Assert.AreEqual(6, result.ChangeLogId);
        }
        
        [Test]
        public void Enqueue_ExistingId_AddsEntity()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            repo.Queue.Enqueue(2);
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
        
        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_builder.ConnectionString).Options;
            using var context = new ArchiveContext(options);

            context.Queue.Add(new QueuedLog(1));
            context.Queue.Add(new QueuedLog(2));
            context.Queue.Add(new QueuedLog(3));
            context.Queue.Add(new QueuedLog(4));
            context.Queue.Add(new QueuedLog(5));

            context.SaveChanges();
        }
    }
}