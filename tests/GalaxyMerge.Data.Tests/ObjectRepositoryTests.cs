using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Test.Core;
using NUnit.Framework;

namespace GalaxyMerge.Data.Tests
{
    [TestFixture]
    public class ObjectRepositoryTests
    {
        private const string HostName = Settings.CurrentTestHost;
        private const string DatabaseName = Settings.CurrentTestGalaxy;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = new SqlConnectionStringBuilder
            {
                DataSource = HostName, 
                InitialCatalog = DatabaseName, 
                IntegratedSecurity = true
            }.ConnectionString;
        }

        [Test]
        [TestCase("$UserDefined")]
        [TestCase("$ViewEngine")]
        [TestCase("$AppEngine")]
        public void FindByTagName_ValidObjects_ReturnsObjectWithCorrectName(string tagName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.Find(tagName).First();
            stopwatch.Stop();
            
            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
        }

        [Test]
        [TestCase("UserDefined")]
        [TestCase("ViewEngine")]
        [TestCase("AppEngine")]
        public void FindByTagName_InvalidObject_ReturnsNull(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.Find(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void Find_ValidId_ReturnsObject()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.Find(x => x.ObjectId == 14);

            Assert.NotNull(result);
            Assert.AreEqual("$UserDefined", result.TagName);
        }

        [Test]
        [TestCase("$UserDefined")]
        [TestCase("$ViewEngine")]
        [TestCase("$AppEngine")]
        public void FindIncludeTemplate_ValidObject_ReturnsObjectWithTemplateNotNull(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindInclude(x => x.TagName == tagName, g => g.TemplateDefinition);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.TemplateDefinition);
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        public void FindIncludeArea_ValidObject_ReturnsObjectWithAreaNotNull(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindInclude(x => x.TagName == tagName, g => g.Area);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Area);
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        public void FindIncludeHost_ValidObject_ReturnsObjectWithHostNotNull(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindInclude(x => x.TagName == tagName, g => g.Host);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Host);
        }

        [Test]
        [TestCase("$UserDefined")]
        public void FindIncludeDerivations_ValidObject_ReturnsObjectWithDerivedObject(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindInclude(x => x.TagName == tagName, g => g.Derivations);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Derivations);
        }

        [Test]
        [TestCase("$UserDefined")]
        public void FindIncludeAllDerivations_ValidObject_ReturnsObjectWithDerivedObject(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindIncludeDescendants(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Derivations);
        }

        [Test]
        public void FindIncludeChangeLogs_WhenCalled_ReturnsObjectWithChangeLogs()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindInclude(x => x.TagName == "$Test_Template", x => x.ChangeLogs);
            
            Assert.NotNull(result);
            Assert.IsNotEmpty(result.ChangeLogs);
        }

        [Test]
        public async Task GetDerivationHierarchy_WhenCalled_ReturnsNotEmpty()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var derivations = (await repo.Objects.GetDerivationHierarchy()).ToList();
            
            Assert.NotNull(derivations);
            Assert.IsNotEmpty(derivations);
        }

        [Test]
        [TestCase("FormatString")]
        public void FindIncludeFolder_ValidSymbol_ReturnsValidFolderObject(string tagName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var result = repo.Objects.FindIncludeFolder(tagName);
            Assert.NotNull(result);
            Assert.NotNull(result.Folder);
        }
    }
}