using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Moq;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ChangeLogProcessorTests
    {
        [Test]
        public void Process_WhenCalled_SomethingHappens()
        {
            var mockProvider = new Mock<IGalaxyDataProvider>()
                .Setup(p => p.Objects.Find(It.IsAny<int>()))
                .Returns(new GObject(1, "", 1));
        }
    }
}