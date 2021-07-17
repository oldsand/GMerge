using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GServer.Archestra;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyFinderTests
    {
        [Test]
        public void Exists_ExistingGalaxy_ReturnsTrue()
        {
            var finder = new GalaxyFinder();

            var exists = finder.Exists(Global.GalaxyName);

            Assert.True(exists);
        }
        
        [Test]
        public void Exists_NonExistingGalaxy_ReturnsFalse()
        {
            var finder = new GalaxyFinder();

            var exists = finder.Exists("This_Is_A_Fake_Haha");

            Assert.False(exists);
        }
        
        [Test]
        public async Task ExistsAsync_ExistingGalaxy_ReturnsTrue()
        {
            var finder = new GalaxyFinder();

            var exists = await finder.ExistsAsync(Global.GalaxyName, CancellationToken.None);

            Assert.True(exists);
        }

        [Test]
        public void FindAll_WhenCalled_ReturnsNonEmptyCollection()
        {
            var finder = new GalaxyFinder();

            var results = finder.FindAll().ToList();
            
            Assert.IsNotEmpty(results);
            Assert.That(results.Count, Has.Count.GreaterThanOrEqualTo(1));
            Assert.That(results, Contains.Item(Global.GalaxyName));
        }
        
        [Test]
        public async Task FindAsync_WhenCalled_ReturnsGalaxyNamesOnCurrentHost()
        {
            var finder = new GalaxyFinder();

            var results = (await finder.FindAllAsync(CancellationToken.None)).ToList();
            
            Assert.IsNotEmpty(results);
            Assert.That(results.Count, Has.Count.GreaterThanOrEqualTo(1));
            Assert.That(results, Contains.Item(Global.GalaxyName));
        }
    }
}