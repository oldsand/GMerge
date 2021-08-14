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

        [Test]
        public void Serialize_WhenCalled_ShouldNotBeNull()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);

            var result = attribute.Serialize();

            result.Should().NotBeNull();
        }

        [Test]
        public void Serialize_WhenCalled_ShouldHaveExpectedAttributes()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);

            var result = attribute.Serialize();

            result.Should().HaveAttribute(nameof(attribute.Name), attribute.Name);
            result.Should().HaveAttribute(nameof(attribute.DataType), attribute.DataType.ToString());
            result.Should().HaveAttribute(nameof(attribute.Category), attribute.Category.ToString());
            result.Should().HaveAttribute(nameof(attribute.Security), attribute.Security.ToString());
            result.Should().HaveAttribute(nameof(attribute.Locked), attribute.Locked.ToString());
            result.Should().HaveAttribute(nameof(attribute.ArrayCount), attribute.ArrayCount.ToString());
        }

        [Test]
        public void Serialize_WhenCalled_ShouldHaveValueElement()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);

            var result = attribute.Serialize();

            result.Should().HaveElement(nameof(attribute.Value));
        }

        [Test]
        public void Materialize_ValidObject_ShouldHaveExpectedValues()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);
            var element = attribute.Serialize();

            var result = ArchestraAttribute.Materialize(element);

            result.Name.Should().Be("Test");
            result.DataType.Should().Be(DataType.Boolean);
            result.Category.Should().Be(AttributeCategory.Writeable_UC_Lockable);
            result.Security.Should().Be(SecurityClassification.Operate);
            result.Locked.Should().Be(LockType.Unlocked);
            result.ArrayCount.Should().Be(-1);
            result.Value.Should().Be(false);
        }
    }
}