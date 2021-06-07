using GalaxyMerge.Client.Data.Repositories;
using NUnit.Framework;

namespace GalaxyMerge.Client.Data.Tests
{
    [TestFixture]
    public class ResourceRepositoryTests
    {
        [Test]
        public void GetAll_WhenCalled_ReturnsExpectedData()
        {
            var repo = new ResourceRepository();
            var results = repo.GetAll();
            
            Assert.NotNull(results);
        }
    }
}