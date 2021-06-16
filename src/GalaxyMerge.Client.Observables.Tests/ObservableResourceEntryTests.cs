using GalaxyMerge.Client.Data.Entities;
using NUnit.Framework;

namespace GalaxyMerge.Client.Observables.Tests
{
    public class ObservableResourceEntryTests
    {
        private ResourceEntry _entry;

        [SetUp]
        public void Setup()
        {
            _entry = new ResourceEntry("Some Name", ResourceType.Connection);
        }

        [Test]
        public void Constructor_WhenConstructed_ReturnsInstance()
        {
            var observable = new ObservableResourceEntry(_entry);
            
            Assert.NotNull(observable);
            Assert.NotNull(observable.Model);
        }
    }
}