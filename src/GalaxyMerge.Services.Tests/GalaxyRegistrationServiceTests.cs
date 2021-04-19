using GalaxyMerge.Archestra;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class GalaxyRegistrationServiceTests
    {
        [Test]
        public void Constructor_WhenCalled_ReturnsNotNull()
        {
            var finder = new GalaxyFinder();
            var registry = new GalaxyRegistry();
            var service = new GalaxyRegistrant(finder, registry);
            Assert.NotNull(service);
        }

        [Test]
        public void Start_WhenCalled_RegistersKnownGalaxies()
        {
            var finder = new GalaxyFinder();
            var registry = new GalaxyRegistry();
            var service = new GalaxyRegistrant(finder, registry);
            
            service.RunRegistration();

            var galaxies = registry.GetAllGalaxies();
            Assert.IsNotEmpty(galaxies);
        }
    }
}