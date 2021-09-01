using FluentAssertions;
using GServer.Archestra.IntegrationTests.Base;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyRepositoryFactoryTests
    {
        [Test]
        public void Create_KnownGalaxyName_ReturnsNotNull()
        {
            var factory = new GalaxyRepositoryFactory();
            var connection = factory.Create(TestConfig.GalaxyName);
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