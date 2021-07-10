using System.Threading;
using System.Threading.Tasks;
using GServer.Archestra;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class GalaxyFactoryTests
    {
        [Test]
        [TestCase("ButaneDev2014")]
        [TestCase("Butane_Production")]
        public void Create_WhenCalled_ReturnsNotNull(string galaxyName)
        {
            var factory = new GalaxyRepositoryFactory();
            var connection = factory.Create(galaxyName);
            Assert.NotNull(connection);
        }
        
        [Test]
        [TestCase("ButaneDev2014")]
        [TestCase("Butane_Production")]
        public async Task CreateAsync_WhenCalled_ReturnsNotNull(string galaxyName)
        {
            var factory = new GalaxyRepositoryFactory();
            var connection = await  factory.CreateAsync(galaxyName, CancellationToken.None);
            Assert.NotNull(connection);
        }
    }
}