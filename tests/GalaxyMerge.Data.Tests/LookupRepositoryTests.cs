using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Testing;
using NUnit.Framework;

namespace GalaxyMerge.Data.Tests
{
    [TestFixture]
    public class LookupRepositoryTests
    {
        [Test]
        [TestCase(13317)] //$Test_Template in ButaneDev2014
        [TestCase(829)] //$AnalogInput_v3 in ButaneDev2014
        public void FindAncestors_ValidObjectId_ReturnsExpectedAncestors(int objectId)
        {
            var repo = new LookupRepository(Settings.CurrentTestGalaxy);

            var ancestors = repo.FindAncestors(objectId);
            
            Assert.IsNotEmpty(ancestors);
        }

        [Test]
        public void FindAncestors_InvalidId_ReturnsEmptyList()
        {
            var repo = new LookupRepository(Settings.CurrentTestGalaxy);

            var ancestors = repo.FindAncestors(0);

            Assert.IsEmpty(ancestors);
        }
    }
}