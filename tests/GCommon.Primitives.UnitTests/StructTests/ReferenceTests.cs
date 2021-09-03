using FluentAssertions;
using GCommon.Primitives.Structs;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.StructTests
{
    [TestFixture]
    public class ReferenceTests
    {
        [Test]
        public void Default_WhenCalled_ShouldHaveExpectedProperties()
        {
            var reference = Reference.Empty();

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(Reference.DefaultReference);
            reference.ObjectName.Should().Be(string.Empty);
            reference.AttributeName.Should().Be(string.Empty);
        }
        
        [Test]
        public void FromName_FullReferenceName_ShouldHaveExpectedProperties()
        {
            const string fullName = "Object_Name.Attribute_Name";
            const string objectName = "Object_Name";
            const string attributeName = "Attribute_Name";
            
            var reference = Reference.FromName(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(objectName);
            reference.AttributeName.Should().Be(attributeName);
        }
        
        [Test]
        public void FromName_FullReferenceNameWithSubProperty_ShouldHaveExpectedProperties()
        {
            const string fullName = "Object_Name.Attribute_Name.Property_Name";
            const string objectName = "Object_Name";
            const string attributeName = "Attribute_Name.Property_Name";
            
            var reference = Reference.FromName(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(objectName);
            reference.AttributeName.Should().Be(attributeName);
        }
        
        [Test]
        public void FromName_RelativeReference_ShouldHaveExpectedProperties()
        {
            const string fullName = "Me.Attribute_Name.Property_Name";
            const string objectName = "Me";
            const string attributeName = "Attribute_Name.Property_Name";
            
            var reference = Reference.FromName(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(objectName);
            reference.AttributeName.Should().Be(attributeName);
        }
        
        [Test]
        public void FromName_OnlyObjectName_ShouldHaveExpectedProperties()
        {
            const string fullName = "ObjectName";

            var reference = Reference.FromName(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(string.Empty);
            reference.AttributeName.Should().Be(string.Empty);
        }
    }
}