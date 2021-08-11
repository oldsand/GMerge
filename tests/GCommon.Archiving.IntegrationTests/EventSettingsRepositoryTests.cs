using System;
using System.IO;
using System.Linq;
using GCommon.Archiving.Repositories;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class EventSettingsRepositoryTests
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
        public void IsTrigger_IsEventTrigger_ReturnsTrue()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Events.IsTrigger(Operation.CreateInstance);

            Assert.True(result);
        }
        
        [Test]
        public void IsTrigger_IsNotEventTrigger_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Events.IsTrigger(Operation.Upload);

            Assert.True(result);
        }
        
        [Test]
        public void IsTrigger_Null_ThrowsArgumentNullException()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = repo.Events.IsTrigger(null);
            }, "operation can not be null");
        }
        
        [Test]
        public void Get_ValidOperation_ReturnsExpectObject()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Events.Get(Operation.DeploySuccess);

            Assert.NotNull(result);
            Assert.AreEqual(Operation.DeploySuccess, result.Operation);
        }
        
        [Test]
        public void Get_Null_ThrowArgumentNullException()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = repo.Events.Get(null);
            }, "operation can not be null");
        }
        
        [Test]
        public void GetAll_WhenCalled_ReturnsNumbersOfExpectedOperations()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var count = Operation.List.Count;

            var operations = repo.Events.GetAll().ToList();
            
            Assert.That(operations, Has.Count.EqualTo(count));
        }

        [Test]
        public void GetArchiveEvents_WhenCalled_ReturnsAllArchiveEvents()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            var operations = repo.Events.GetArchiveEvents().ToList();
            
            Assert.True(operations.All(x => x.IsArchiveEvent));
        }

        [Test]
        public void Configure_SetTriggerTrue_ReturnsTrue()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            repo.Events.Configure(Operation.DeploySuccess, true);
            repo.Save();

            var result = repo.Events.Get(Operation.DeploySuccess);
            
            Assert.True(result.IsArchiveEvent);
        }
        
        [Test]
        public void Configure_SetTriggerFalse_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            repo.Events.Configure(Operation.CheckInSuccess, false);
            repo.Save();

            var result = repo.Events.Get(Operation.CheckInSuccess);
            
            Assert.False(result.IsArchiveEvent);
        }
        
        [Test]
        public void Configure_NullOperation_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                repo.Events.Configure(null, false);
            }, "operation can not be null");
        }
    }
}