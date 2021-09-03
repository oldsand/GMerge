using FluentAssertions;
using GCommon.Primitives.Structs;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.StructTests
{
    [TestFixture]
    public class BlobTests
    {
        [Test]
        public void Empty_WhenCalled_ShouldHaveExpectedProperties()
        {
            var blob = Blob.Empty();

            blob.Should().NotBeNull();
            blob.Data.Should().NotBeNull();
            blob.Data.Should().BeEmpty();
            blob.Guid.Should().Be(-1);
        }
    }
}