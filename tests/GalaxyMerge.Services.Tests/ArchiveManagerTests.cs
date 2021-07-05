using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Data.Abstractions;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ArchiveManagerTests
    {

        [SetUp]
        public void Setup()
        {
            
        }
        
        [Test]
        public void GetArchiveEntryAndConvertToGalaxyObjectData()
        {
            var manager = new ArchiveManager(new GalaxyRegistry(), new GalaxyDataRepositoryFactory());
            manager.Connect("ButaneDev2014");
            
            var data = manager.GetGalaxyObject(46885);
            
            Assert.NotNull(data);
        }
    }
}