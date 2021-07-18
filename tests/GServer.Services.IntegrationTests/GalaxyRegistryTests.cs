using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTest.Core;
using NUnit.Framework;

namespace GServer.Services.IntegrationTests
{
    [TestFixture]
    public class GalaxyRegistryTests
    {
        [Test]
        public void RegisterGalaxy_WhenCalled_GetGalaxyReturnValidGalaxy()
        {
            var registry = new GalaxyRegistry();

            registry.Register("ButaneDev2014", "admin");

            var connection = registry.GetGalaxy("ButaneDev2014", "admin");
            
            Assert.NotNull(connection);
            Assert.IsTrue(connection.Name == "ButaneDev2014");
            Assert.IsTrue(connection.ConnectedUser == "admin");
        }

        [Test]
        public void RegisterAll_WhenCalled_ReturnsAll()
        {
            var registry = new GalaxyRegistry();
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            registry.RegisterAll();
            stopWatch.Stop();

            var results = registry.GetAll();
            Assert.IsNotEmpty(results);
        }
    }
}
