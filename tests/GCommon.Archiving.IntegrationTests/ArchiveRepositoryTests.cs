using System;
using System.IO;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GCommon.Archiving;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
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
            Assert.AreEqual(ArchestraVersion.SystemPlatform2012R2P3, archive.Version);
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

        /*private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_connectionString).Options;
            using var context = new ArchiveContext(options);

            context.Objects.Add(new ArchiveObject(1, "Tag1", 21, Template.UserDefined));
            context.Objects.Add(new ArchiveObject(2, "Tag2", 33, Template.Area));
            context.Objects.Add(new ArchiveObject(3, "Tag3", 13, Template.Symbol));

            var archiveObject = new ArchiveObject(311, "TestObject", 2, Template.UserDefined);
            archiveObject.AddEntry(Encoding.UTF8.GetBytes("This is some data to convert to binary"), 34523);
            context.Objects.Add(archiveObject);

            context.Queue.Add(new QueuedEntry(132432, 2, 0, DateTime.Today));
            context.Queue.Add(new QueuedEntry(123523, 311, 4, DateTime.Now));
            context.Queue.Add(new QueuedEntry(577854, 3, 14, DateTime.Today.AddHours(1)));

            context.SaveChanges();
        }*/
    }
}