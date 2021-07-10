using System;
using System.IO;
using System.Linq;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Archiving;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class EventSettingsRepositoryTests
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
        public void IsTrigger_IsEventTrigger_ReturnsTrue()
        {
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Events.IsTrigger(Operation.CreateInstance);

            Assert.True(result);
        }
        
        [Test]
        public void IsTrigger_IsNotEventTrigger_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Events.IsTrigger(Operation.Upload);

            Assert.True(result);
        }
        
        [Test]
        public void IsTrigger_Null_ThrowsArgumentNullException()
        {
            using var repo = new ArchiveRepository(_connectionString);
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = repo.Events.IsTrigger(null);
            }, "operation can not be null");
        }
        
        [Test]
        public void Get_ValidOperation_ReturnsExpectObject()
        {
            using var repo = new ArchiveRepository(_connectionString);

            var result = repo.Events.Get(Operation.DeploySuccess);

            Assert.NotNull(result);
            Assert.AreEqual(Operation.DeploySuccess, result.Operation);
        }
        
        [Test]
        public void Get_Null_ThrowArgumentNullException()
        {
            using var repo = new ArchiveRepository(_connectionString);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = repo.Events.Get(null);
            }, "operation can not be null");
        }
        
        [Test]
        public void GetAll_WhenCalled_ReturnsNumbersOfExpectedOperations()
        {
            using var repo = new ArchiveRepository(_connectionString);
            var count = Enumeration.GetAll<Operation>().ToList().Count;

            var operations = repo.Events.GetAll().ToList();
            
            Assert.That(operations, Has.Count.EqualTo(count));
        }

        [Test]
        public void GetArchiveEvents_WhenCalled_ReturnsAllArchiveEvents()
        {
            using var repo = new ArchiveRepository(_connectionString);
            
            var operations = repo.Events.GetArchiveEvents().ToList();
            
            Assert.True(operations.All(x => x.IsArchiveEvent));
        }

        [Test]
        public void Configure_SetTriggerTrue_ReturnsTrue()
        {
            using var repo = new ArchiveRepository(_connectionString);
            
            repo.Events.Configure(Operation.DeploySuccess, true);
            repo.Save();

            var result = repo.Events.Get(Operation.DeploySuccess);
            
            Assert.True(result.IsArchiveEvent);
        }
        
        [Test]
        public void Configure_SetTriggerFalse_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_connectionString);
            
            repo.Events.Configure(Operation.CheckInSuccess, false);
            repo.Save();

            var result = repo.Events.Get(Operation.CheckInSuccess);
            
            Assert.False(result.IsArchiveEvent);
        }
        
        [Test]
        public void Configure_NullOperation_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_connectionString);
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                repo.Events.Configure(null, false);
            }, "operation can not be null");
        }
    }
}