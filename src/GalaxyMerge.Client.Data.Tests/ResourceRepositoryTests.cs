using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Data.Repositories;
using GalaxyMerge.Client.Data.Tests.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GalaxyMerge.Client.Data.Tests
{
    [TestFixture]
    public class ResourceRepositoryTests : SqliteTestFixture<AppContext>
    {
        [SetUp]
        public void Setup()
        {
            using var context = new AppContext(ContextOptions);
            Create(context);
        }
        
        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        protected override void Seed()
        {
            using var context = new AppContext(ContextOptions);
            context.Resources.Add(new ResourceEntry("Resource1", ResourceType.Connection, "This is a test"));
            context.Resources.Add(new ResourceEntry("Resource2", ResourceType.Archive, "Resource Number 2"));
            context.Resources.Add(new ResourceEntry("Resource3", ResourceType.Directory, "Test Description"));
            context.Resources.Add(new ResourceEntry("Resource4", ResourceType.Connection, "This is another test"));
            context.SaveChanges();
        }

        [Test]
        public void Get_ResourceId_ReturnExpectedEntry()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);
            
            var result = repo.Get(1);
            
            Assert.NotNull(result);
            Assert.AreEqual("Resource1", result.ResourceName);
            Assert.AreEqual(ResourceType.Connection, result.ResourceType);
            Assert.AreEqual( "This is a test", result.ResourceDescription);
        }
        
        [Test]
        public void Get_ResourceName_ReturnExpectedEntry()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);
            
            var result = repo.Get("Resource3");
            
            Assert.NotNull(result);
            Assert.AreEqual("Resource3", result.ResourceName);
            Assert.AreEqual(ResourceType.Directory, result.ResourceType);
            Assert.AreEqual( "Test Description", result.ResourceDescription);
        }

        [Test]
        public void Get_DoesNotExist_ReturnsNull()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);
            
            var result = repo.Get("Resource23");
            
            Assert.Null(result);
        }
        
        [Test]
        public void GetAll_WhenCalled_ReturnsAllEntries()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);

            var entries = repo.GetAll().ToList();
            
            Assert.NotNull(entries);
            Assert.IsNotEmpty(entries);
            Assert.That(entries, Has.Count.EqualTo(4));
        }
        
        [Test]
        public void GetNames_WhenCalled_ReturnsAllResourceNames()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);

            var entries = repo.GetAll().ToList();
            
            Assert.NotNull(entries);
            Assert.IsNotEmpty(entries);
            Assert.That(entries, Has.Count.EqualTo(4));
        }
        
        [Test]
        public async Task GetAllAsync_WhenCalled_ReturnsAllEntries()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);

            var entries = (await repo.GetAllAsync()).ToList();
            
            Assert.NotNull(entries);
            Assert.IsNotEmpty(entries);
            Assert.That(entries, Has.Count.EqualTo(4));
        }
        
        [Test]
        public void Add_WhenCalled_CanGetAddedEntry()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);

            repo.Add(new ResourceEntry("TestAddResource", ResourceType.Archive));
            repo.Save();

            var result = repo.Get("TestAddResource");

            Assert.NotNull(result);
            Assert.AreEqual("TestAddResource", result.ResourceName);
            Assert.AreEqual(ResourceType.Archive, result.ResourceType);
            Assert.Null(result.ResourceDescription);
        }
        
        [Test]
        public void Add_ExistingName_ThrowSqlException()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);
            
            Assert.Throws<DbUpdateException>(() =>
            {
                repo.Add(new ResourceEntry("Resource1", ResourceType.Archive));
                repo.Save();
            });
        }
        
        [Test]
        public void Update_WhenCalled_UpdatePropertiesReturnExpected()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);

            var target = repo.Get(4);
            target.ResourceName = "Resource44";
            target.ResourceDescription = "TestUpdate";
            
            repo.Update(target);
            repo.Save();

            var result = repo.Get(4);

            Assert.NotNull(result);
            Assert.AreEqual("Resource44", result.ResourceName);
            Assert.AreEqual(ResourceType.Connection, result.ResourceType);
            Assert.AreEqual("TestUpdate", result.ResourceDescription);
        }
        
        [Test]
        public void Update_ExistingName_ThrowSqlException()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);
            var target = repo.Get(4);
            target.ResourceName = "Resource2";

            Assert.Throws<DbUpdateException>(() =>
            {
                repo.Update(target);
                repo.Save();
            });
        }
        
        [Test]
        public void Remove_WhenCalled_GetReturnsNull()
        {
            Seed();
            using var repo = new ResourceRepository(Connection);

            var target = repo.Get(2);
            
            repo.Remove(target);
            repo.Save();

            var result = repo.Get(2);

            Assert.Null(result);
        }
    }
}