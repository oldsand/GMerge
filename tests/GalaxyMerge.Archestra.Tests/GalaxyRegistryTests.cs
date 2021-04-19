using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class GalaxyRegistryTests
    {
        [Test]
        public void RegisterGalaxy_WhenCalled_GetGalaxyReturnValidGalaxy()
        {
            var registry = new GalaxyRegistry();

            registry.RegisterGalaxy("ButaneDev2014", "admin");

            var connection = registry.GetGalaxy("ButaneDev2014", "admin");
            
            Assert.NotNull(connection);
            Assert.IsTrue(connection.Name == "ButaneDev2014");
            Assert.IsTrue(connection.LoggedInUser == "admin");
        }

        [Test]
        public async Task RegisterGalaxyAsync_ValidGalaxy_CanReturnProvidedGalaxy()
        {
            var registry = new GalaxyRegistry();

            await registry.RegisterGalaxyAsync(Settings.CurrentTestGalaxy, Settings.CurrentTestUser, CancellationToken.None);

            var connection = registry.GetGalaxy(Settings.CurrentTestGalaxy, Settings.CurrentTestUser);
            
            Assert.NotNull(connection);
            Assert.IsTrue(connection.Name == Settings.CurrentTestGalaxy);
            Assert.IsTrue(connection.LoggedInUser == Settings.CurrentTestUser);
        }

        [Test]
        public async Task RegisterGalaxiesAsync_WhenCalled_RegistryContainsAllGalaxies()
        {
            var registry = new GalaxyRegistry();

            await registry.RegisterGalaxiesAsync(Settings.CurrentTestUser, CancellationToken.None);

            var galaxies = registry.GetAllGalaxies().ToList();
            
            Assert.AreEqual(3, galaxies.Count);
            Assert.True(galaxies.Any(x => x.Name == Settings.CurrentTestGalaxy));
        }
    }
}
