using System;
using System.Data.SqlClient;
using GalaxyMerge.Data.Repositories;
using NUnit.Framework;

namespace GalaxyMerge.Data.Tests
{
    [TestFixture]
    public class ObjectRepositoryTests
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = new SqlConnectionStringBuilder
            {
                DataSource = Environment.MachineName, InitialCatalog = "ButaneDev2014", IntegratedSecurity = true
            }.ConnectionString;
        }

        [Test]
        [TestCase("$UserDefined")]
        [TestCase("$ViewEngine")]
        [TestCase("$AppEngine")]
        public void FindByTagName_ValidObject_ReturnsObjectWithCorrectName(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindByTagName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
        }

        [Test]
        [TestCase("UserDefined")]
        [TestCase("ViewEngine")]
        [TestCase("AppEngine")]
        public void FindByTagName_InvalidObject_ReturnsNull(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindByTagName(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void Find_ValidId_ReturnsObject()
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.Find(x => x.ObjectId == 14);

            Assert.NotNull(result);
            Assert.AreEqual("$UserDefined", result.TagName);
        }

        [Test]
        [TestCase("$UserDefined")]
        [TestCase("$ViewEngine")]
        [TestCase("$AppEngine")]
        public void FindIncludeTemplate_ValidObject_ReturnsObjectWithTemplateNotNull(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Template);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Template);
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        public void FindIncludeArea_ValidObject_ReturnsObjectWithAreaNotNull(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Area);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Area);
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        public void FindIncludeHost_ValidObject_ReturnsObjectWithHostNotNull(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Host);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Host);
        }

        [Test]
        [TestCase("$UserDefined")]
        public void FindIncludeDerivations_ValidObject_ReturnsObjectWithDerivedObject(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Derivations);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Derivations);
        }

        [Test]
        [TestCase("$UserDefined")]
        public void FindIncludeAllDerivations_ValidObject_ReturnsObjectWithDerivedObject(string tagName)
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindIncludeDescendants(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Derivations);
        }

        [Test]
        public void FindIncludeChangeLogs_WhenCalled_ReturnsObjectWithChangeLogs()
        {
            var repo = new ObjectRepository(_connectionString);

            var result = repo.FindInclude(x => x.TagName == "$Test_Template", x => x.ChangeLogs);
            
            Assert.NotNull(result);
            Assert.IsNotEmpty(result.ChangeLogs);
        }
    }
}