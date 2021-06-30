using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Data.Abstractions;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ArchiveManagerTests
    {
        private GalaxyRepositoryProvider _provider;

        [SetUp]
        public void Setup()
        {
            var registry = new GalaxyRegistry();
            registry.Register("ButaneDev2014");
            _provider = new GalaxyRepositoryProvider(registry);
        }
        
        [Test]
        public void GetArchiveEntryAndConvertToGalaxyObjectData()
        {
            var manager = new ArchiveManager(_provider, new GalaxyDataRepositoryFactory());
            manager.Connect("ButaneDev2014");
            
            var data = manager.GetGalaxyObject(46885);
            
            Assert.NotNull(data);
        }
    }
}