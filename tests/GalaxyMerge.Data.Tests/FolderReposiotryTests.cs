using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;

namespace GalaxyMerge.Data.Tests
{
    [TestFixture]
    public class FolderRepositoryTests
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
        [TestCase("EN - Functions")]
        public void Find_ValidName_ReturnsExpectedFolderName(string folderName)
        {
            var repo = new FolderRepository(_connectionStringBuilder);

            var folder = repo.Find(x => x.FolderName == folderName);
            
            Assert.NotNull(folder);
            Assert.AreEqual(folderName, folder.FolderName);
        }
        
        [Test]
        public void FindInclude_ChildFolders_ReturnsExpectedChildFolder()
        {
            var repo = new FolderRepository(_connectionStringBuilder);

            var folder = repo.FindInclude(x => x.FolderName == "ArchestrA Symbol Library", x => x.Folders);
            
            Assert.IsNotEmpty(folder.Folders);
            Assert.True(folder.Folders.Any(x => x.FolderName == "Buttons"));
            Assert.True(folder.Folders.Any(x => x.FolderName == "Clocks"));
        }
        
        [Test]
        public async Task GetSymbolHierarchy_WhenCalled_ReturnsNotEmpty()
        {
            var repo = new FolderRepository(_connectionStringBuilder);

            var symbols = (await repo.GetSymbolHierarchy()).ToList();
            
            Assert.NotNull(symbols);
            Assert.True(symbols.Any(x => x.Objects.Any()));
        }
    }
}