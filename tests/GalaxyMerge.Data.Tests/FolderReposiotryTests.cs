using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Test.Core;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace GalaxyMerge.Data.Tests
{
    [TestFixture]
    public class FolderRepositoryTests
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
        [TestCase("EN - Functions")]
        public void Find_ValidName_ReturnsExpectedFolderName(string folderName)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var folder = repo.FoldersReadOnly.Find(x => x.FolderName == folderName);
            
            Assert.NotNull(folder);
            Assert.AreEqual(folderName, folder.FolderName);
        }
        
        [Test]
        public void FindInclude_ChildFolders_ReturnsExpectedChildFolder()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var folder = repo.FoldersReadOnly.FindInclude(x => x.FolderName == "ArchestrA Symbol Library", x => x.Folders);
            
            Assert.IsNotEmpty(folder.Folders);
            Assert.True(folder.Folders.Any(x => x.FolderName == "Buttons"));
            Assert.True(folder.Folders.Any(x => x.FolderName == "Clocks"));
        }
        
        [Test]
        public async Task GetSymbolHierarchy_WhenCalled_ReturnsNotEmpty()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var symbols = (await repo.FoldersReadOnly.GetSymbolHierarchy()).ToList();
            
            Assert.NotNull(symbols);
            Assert.True(symbols.Any(x => x.Objects.Any()));
        }
    }
}