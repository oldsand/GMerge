using System;
using System.IO;
using System.Linq;
using System.Text;
using GCommon.Archiving.Repositories;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class ArchiveLogRepositoryTests
    {
        private SqliteConnectionStringBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SqliteConnectionStringBuilder {DataSource = @"\TestArchive.db"};

            var archive = new Archive("TestArchive");

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
        public void Get_ExistingLog_ReturnsExpectedEntity()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Logs.Get(1);

            Assert.NotNull(result);
            Assert.NotNull(result.ArchiveObject);
            Assert.AreEqual(1, result.ChangeLogId);
            Assert.AreEqual(DateTime.Today, result.ChangedOn);
            Assert.AreEqual(Operation.CreateInstance, result.Operation);
            Assert.AreEqual("Created new instance", result.Comment);
            Assert.AreEqual(Environment.UserName, result.UserName);
        }

        [Test]
        public void Get_NonExistingLog_ReturnsReturnsNull()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Logs.Get(10);

            Assert.Null(result);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsExpectedCount()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.Logs.GetAll().ToList();

            Assert.That(results, Has.Count.EqualTo(4));
        }

        [Test]
        public void GetAll_EmptyDatabase_ReturnsIsEmpty()
        {
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.Logs.GetAll().ToList();

            Assert.IsEmpty(results);
        }

        [Test]
        public void Find_ByOperation_ReturnsExpectedEntity()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.Logs.Find(x => x.Operation == Operation.CreateInstance).ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.All(x => x.Operation == Operation.CreateInstance));
        }

        [Test]
        public void Find_ByDateTime_ReturnsExpectedEntities()
        {
            Seed();
            var repo = new ArchiveRepository(_builder.ConnectionString);

            var results = repo.Logs.Find(x => x.ChangedOn == DateTime.Today).ToList();

            Assert.That(results, Has.Count.EqualTo(1));
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_builder.ConnectionString).Options;
            using var context = new ArchiveContext(options);

            var obj = new ArchiveObject(1, "Tag1", 21, Template.UserDefined);
            context.Objects.Add(obj);

            context.Logs.Add(new ArchiveLog(1, new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test1")), DateTime.Today,
                Operation.CreateInstance, "Created new instance", Environment.UserName));
            context.Logs.Add(new ArchiveLog(2, new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test2")),
                DateTime.Today.AddHours(2), Operation.CheckInSuccess, "Updated current object", Environment.UserName));
            context.Logs.Add(new ArchiveLog(3, new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test3")),
                DateTime.Today.AddHours(3), Operation.Rename, "Renamed instance", Environment.UserName));
            context.Logs.Add(new ArchiveLog(4, new ArchiveEntry(obj, Encoding.UTF8.GetBytes("Test4")),
                DateTime.Today.AddHours(4), Operation.ModifiedAutomationObjectOnly, "Modified something",
                Environment.UserName));

            context.SaveChanges();
        }
    }
}