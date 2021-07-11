using System.IO;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class QueueRepositoryTests
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
    }
}