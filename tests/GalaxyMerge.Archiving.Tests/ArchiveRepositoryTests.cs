using System.IO;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GalaxyMerge.Archiving.Tests
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
        

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ArchiveContext>()
                .UseSqlite(_connectionString).Options;
            using var context = new ArchiveContext(options);
            
            //context.
        }
    }
}