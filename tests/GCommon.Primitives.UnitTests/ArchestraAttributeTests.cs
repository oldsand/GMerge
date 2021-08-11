using FluentAssertions;
using GCommon.Primitives.Enumerations;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests
{
    [TestFixture]
    public class ArchestraAttributeTests
    {

        [Test]
        public void Construct_NameAndDataType_ShouldNotBeNull()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);

            attribute.Should().NotBeNull();
            attribute.Name.Should().Be("Test");
            attribute.DataType.Should().Be(DataType.Boolean);
        }
    }
}