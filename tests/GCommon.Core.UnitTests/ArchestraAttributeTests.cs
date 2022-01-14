using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using GCommon.Differencing;
using GCommon.Core.Enumerations;
using NUnit.Framework;

namespace GCommon.Core.UnitTests
{
    [TestFixture]
    public class ArchestraAttributeTests
    {

        [Test]
        public void New_NameAndBoolean_ShouldNotBeNull()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);

            attribute.Should().NotBeNull();
            attribute.Name.Should().Be("Test");
            attribute.DataType.Should().Be(DataType.Boolean);
            attribute.Category.Should().Be(AttributeCategory.Writeable_UC_Lockable);
            attribute.Security.Should().Be(SecurityClassification.Operate);
            attribute.ArrayCount.Should().Be(-1);
            attribute.IsArray.Should().Be(false);
            attribute.Value.Should().Be(false);
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
        [UseReporter(typeof(DiffReporter))]
        public void Serialize_WhenCalled_ShouldHaveApprovedValue()
        {
            var attribute = new ArchestraAttribute("Test", DataType.Boolean);

            var result = attribute.Serialize();
            
            Approvals.VerifyXml(result.ToString());
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

        [Test]
        public void DiffersFrom_DifferentName_ShouldReturnExpected()
        {
            var att1 = new ArchestraAttribute("Name1", DataType.Boolean);
            var att2 = new ArchestraAttribute("Name2", DataType.Boolean);

            var differences = att1.DiffersFrom(att2).ToList(); 

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be("Name1");
            differences.First().Right.Should().Be("Name2");
            differences.First().PropertyName.Should().Be(nameof(att1.Name));
            differences.First().PropertyType.Should().Be(typeof(string));
            differences.First().ObjectType.Should().Be(typeof(ArchestraAttribute));
            differences.First().DifferenceType.Should().Be(DifferenceType.Changed);
        }
    }
}