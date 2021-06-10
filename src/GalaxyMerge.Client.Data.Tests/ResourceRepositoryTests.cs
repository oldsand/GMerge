using System.Linq;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Data.Repositories;
using NUnit.Framework;

namespace GalaxyMerge.Client.Data.Tests
{
    [TestFixture]
    public class ResourceRepositoryTests
    {
        [TearDown]
        public void TearDown()
        {
            using var context = new AppContext();
            var resources = context.Resources.ToList();
            context.Resources.RemoveRange(resources);
            context.SaveChanges();
        }
        
        [Test]
        public void Add_WhenCalled_CanReturnAddedResource()
        {
            using var repo = new ResourceRepository();
            
            repo.Add(new GalaxyResource("TestResource", ResourceType.Connection));
            repo.Save();

            var result = repo.Get("TestResource");
            Assert.NotNull(result);
            Assert.AreEqual("TestResource", result.ResourceName);
            Assert.AreEqual(ResourceType.Connection, result.ResourceType);
        }
    }
}