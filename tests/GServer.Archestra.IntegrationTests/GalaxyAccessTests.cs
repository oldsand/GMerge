using System;
using System.Linq;
using GServer.Archestra.Exceptions;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyAccessTests
    {
        [Test]
        public void CreateAndDestroy_ValidGalaxyName_CanFindGalaxy()
        {
            var galaxyAccess = new GalaxyAccess();

            var results = galaxyAccess.Find().ToList();
            Assert.That(results, Does.Not.Contain("Galaxy_Test"));

            galaxyAccess.Create("Galaxy_Test");

            results = galaxyAccess.Find().ToList();
            Assert.That(results, Does.Contain("Galaxy_Test"));

            galaxyAccess.Destroy("Galaxy_Test");

            results = galaxyAccess.Find().ToList();
            Assert.That(results, Does.Not.Contain("Galaxy_Test"));
        }

        [Test]
        public void Create_NullArgument_ThrowsArgumentNullException()
        {
            var galaxyAccess = new GalaxyAccess();

            Assert.Throws<ArgumentNullException>(() => { galaxyAccess.Create(null); });
        }

        [Test]
        public void Create_ExistingGalaxy_ThrowsGalaxyException()
        {
            var galaxyAccess = new GalaxyAccess();

            galaxyAccess.Create("Some_Test_Galaxy");

            Assert.Throws<GalaxyException>(() => { galaxyAccess.Create("Some_Test_Galaxy"); });

            galaxyAccess.Destroy("Some_Test_Galaxy");

            var results = galaxyAccess.Find().ToList();
            Assert.That(results, Does.Not.Contain("Some_Test_Galaxy"));
        }

        [Test]
        public void Destroy_NullArgument_ThrowsArgumentNullException()
        {
            var galaxyAccess = new GalaxyAccess();

            Assert.Throws<ArgumentNullException>(() => { galaxyAccess.Destroy(null); });
        }

        [Test]
        public void Exists_ExistingGalaxy_ReturnsTrue()
        {
            var galaxyAccess = new GalaxyAccess();

            var exists = galaxyAccess.Exists(TestContext.GalaxyName);

            Assert.True(exists);
        }

        [Test]
        public void Exists_NonExistingGalaxy_ReturnsFalse()
        {
            var galaxyAccess = new GalaxyAccess();

            var exists = galaxyAccess.Exists("This_Is_A_Fake_Haha");

            Assert.False(exists);
        }

        [Test]
        public void Find_WhenCalled_ReturnsNonEmptyCollection()
        {
            var galaxyAccess = new GalaxyAccess();

            var results = galaxyAccess.Find().ToList();

            Assert.IsNotEmpty(results);
            Assert.That(results, Has.Count.GreaterThanOrEqualTo(1));
            Assert.That(results, Contains.Item(TestContext.GalaxyName));
        }
    }
}