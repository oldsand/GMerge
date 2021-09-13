using System.Linq;
using GClient.Data.Entities;
using GClient.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using NUnit.Framework;

namespace GClient.Data.UnitTests
{
    [TestFixture]
    public class LogRepositoryTests
    {
        private const string DatabasePath = "Filename=:memory:";
        private DbContextOptions<AppContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<AppContext>().UseSqlite(DatabasePath).Options;
            using var context = new AppContext(_options);
            context.Database.EnsureCreated();
        }

        [Test]
        public void CreateRepo()
        {
            using var repo = new LogRepository(DatabasePath);
            Assert.NotNull(repo);
        }

        [Test]
        public void Get_Seeded_LogEntry()
        {
            SeedData();
            using var repo = new LogRepository(DatabasePath);
            var result = repo.Find(x => x.Message == "This is a test").FirstOrDefault();
            
            Assert.NotNull(result);
            Assert.AreEqual("This is a test", result.Message);
        }

        private void SeedData()
        {
            using var context = new AppContext(_options);
            context.Logs.Add(new LogEntry(LogLevel.Debug, "This is a test"));
            context.SaveChanges();
        }
    }
}