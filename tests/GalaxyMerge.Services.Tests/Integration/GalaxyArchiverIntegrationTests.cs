using System;
using GServer.Archestra;
using GCommon.Archiving;
using GCommon.Archiving.Repositories;
using GCommon.Primitives;
using GalaxyMerge.Test.Core;
using GServer.Services;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests.Integration
{
    [TestFixture]
    public class GalaxyArchiverIntegrationTests
    {
        private ArchiveRepository _archiveRepo;
        private GalaxyRepository _galaxyRepo;

        [SetUp]
        public void Setup()
        {
            var testConnectionString = new SqliteConnectionStringBuilder 
                {DataSource = $".\\{Settings.CurrentTestGalaxy}.db"}.ConnectionString;
            var testConfig = ArchiveConfiguration
                .Default(Settings.CurrentTestGalaxy, ArchestraVersion.SystemPlatform2014)
                .OverrideConnectionString(testConnectionString);
            var builder = new ArchiveBuilder();
            builder.Build(testConfig);
            
            _galaxyRepo = new GalaxyRepository(Settings.CurrentTestGalaxy);
            _galaxyRepo.Login(Environment.UserName);
            _archiveRepo = new ArchiveRepository(testConnectionString);
        }
        [Test]
        public void Construction_WhenCalled_ReturnsNotNull()
        {
            using var archiver = new GalaxyArchiver(_galaxyRepo, _archiveRepo);
            
            Assert.NotNull(archiver);
        }
    }
}