using System.Linq;
using System.Security.Principal;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class GalaxyRepositoryProviderTests
    {
        private GalaxyRegistry _registry;

        [SetUp]
        public void Setup()
        {
            _registry = new GalaxyRegistry();
            _registry.RegisterAll();
        }
        
        [Test]
        public void GetServiceInstance_WhenCalled_ReturnsInstanceWithCorrectUserName()
        {
            var provider = new GalaxyRepositoryProvider(_registry);

            var instance = provider.GetServiceInstance("Sandbox");

            Assert.NotNull(instance);
            Assert.True(instance.Connected);
            Assert.AreEqual(instance.ConnectedUser, WindowsIdentity.GetCurrent().Name);
        }
        
        [Test]
        public void GetAllServiceInstances_WhenCalled_ReturnsInstanceWithCorrectUserName()
        {
            var provider = new GalaxyRepositoryProvider(_registry);

            var instances = provider.GetAllServiceInstances().ToList();

            Assert.IsNotEmpty(instances);
            Assert.That(instances.Count, Is.GreaterThan(1));
            Assert.That(instances.Any(x => x.Name == "Sandbox"));
            Assert.True(instances.All(x => x.Connected));
            Assert.True(instances.All(x => x.ConnectedUser == WindowsIdentity.GetCurrent().Name));
        }
    }
}