using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GServer.Archestra;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class GalaxyFinderTests
    {
        [Test]
        public async Task FindAsync_WhenCalled_ReturnsGalaxyNamesOnCurrentHost()
        {
            var connectionFinder = new GalaxyFinder();

            var connections = (await connectionFinder.FindAllAsync(CancellationToken.None)).ToList();
            
            Assert.IsNotEmpty(connections);
            Assert.AreEqual(2, connections.Count);
            CollectionAssert.Contains(connections, "ButaneDev2014");
            CollectionAssert.Contains(connections, "Butane_Production");
        }
    }
}