using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Data.Repositories;
using NUnit.Framework;

namespace GalaxyMerge.Data.Tests
{
    [TestFixture]
    public class ObjectRepositoryTests
    {
        private const string HostName = "ETDEVGR1";
        private const string DatabaseName = "ButaneDev2014";
        private SqlConnectionStringBuilder _connectionStringBuilder;

        [SetUp]
        public void Setup()
        {
            _connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = HostName, 
                InitialCatalog = DatabaseName, 
                IntegratedSecurity = true
            };
        }

        [Test]
        [TestCase("$UserDefined")]
        [TestCase("$ViewEngine")]
        [TestCase("$AppEngine")]
        public void FindByTagName_ValidObject_ReturnsObjectWithCorrectName(string tagName)
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

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
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindByTagName(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void Find_ValidId_ReturnsObject()
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

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
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.TemplateDefinition);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.TemplateDefinition);
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        public void FindIncludeArea_ValidObject_ReturnsObjectWithAreaNotNull(string tagName)
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Area);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Area);
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        public void FindIncludeHost_ValidObject_ReturnsObjectWithHostNotNull(string tagName)
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Host);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Host);
        }

        [Test]
        [TestCase("$UserDefined")]
        public void FindIncludeDerivations_ValidObject_ReturnsObjectWithDerivedObject(string tagName)
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindInclude(x => x.TagName == tagName, g => g.Derivations);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Derivations);
        }

        [Test]
        [TestCase("$UserDefined")]
        public void FindIncludeAllDerivations_ValidObject_ReturnsObjectWithDerivedObject(string tagName)
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindIncludeDescendants(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
            Assert.NotNull(result.Derivations);
        }

        [Test]
        public void FindIncludeChangeLogs_WhenCalled_ReturnsObjectWithChangeLogs()
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindInclude(x => x.TagName == "$Test_Template", x => x.ChangeLogs);
            
            Assert.NotNull(result);
            Assert.IsNotEmpty(result.ChangeLogs);
        }

        [Test]
        public async Task GetDerivationHierarchy_WhenCalled_ReturnsNotEmpty()
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var derivations = (await repo.GetDerivationHierarchy()).ToList();
            
            Assert.NotNull(derivations);
            Assert.IsNotEmpty(derivations);
        }

        [Test]
        [TestCase("FormatString")]
        public void FindIncludeFolder_ValidSymbol_ReturnsValidFolderObject(string tagName)
        {
            var repo = new ObjectRepository(_connectionStringBuilder);

            var result = repo.FindIncludeFolder(tagName);
            Assert.NotNull(result);
            Assert.NotNull(result.Folder);
        }
    }
}