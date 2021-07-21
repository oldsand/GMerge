using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GServer.Archestra;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyFactoryTests
    {
        [Test]
        public void Create_KnownGalaxyName_ReturnsNotNull()
        {
            var factory = new GalaxyRepositoryFactory();
            var connection = factory.Create(TestContext.GalaxyName);
            Assert.NotNull(connection);
        }

        [Test]
        public void CreateAll_WhenCalled_ReturnsNonEmptyCollection()
        {
            var factory = new GalaxyRepositoryFactory();
            
            var connections = factory.CreateAll();
            
            connections.Should().NotBeEmpty();
        }
    }
}