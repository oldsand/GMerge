using FluentAssertions;
using GCommon.Core.Structs;
using NUnit.Framework;

namespace GCommon.Core.UnitTests.StructTests
{
    [TestFixture]
    public class ReferenceTests
    {
        [Test]
        public void Default_WhenCalled_ShouldHaveExpectedProperties()
        {
            var reference = Reference.Empty;

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(Reference.DefaultReference);
            reference.ObjectName.Should().Be(string.Empty);
            reference.AttributeName.Should().Be(string.Empty);
        }
        
        [Test]
        public void New_FullReferenceName_ShouldHaveExpectedProperties()
        {
            const string fullName = "Object_Name.Attribute_Name";
            const string objectName = "Object_Name";
            const string attributeName = "Attribute_Name";
            
            var reference = new Reference(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(objectName);
            reference.AttributeName.Should().Be(attributeName);
        }
        
        [Test]
        public void New_FullReferenceNameWithSubProperty_ShouldHaveExpectedProperties()
        {
            const string fullName = "Object_Name.Attribute_Name.Property_Name";
            const string objectName = "Object_Name";
            const string attributeName = "Attribute_Name.Property_Name";
            
            var reference = new Reference(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(objectName);
            reference.AttributeName.Should().Be(attributeName);
        }
        
        [Test]
        public void New_RelativeReference_ShouldHaveExpectedProperties()
        {
            const string fullName = "Me.Attribute_Name.Property_Name";
            const string objectName = "Me";
            const string attributeName = "Attribute_Name.Property_Name";
            
            var reference = new Reference(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(objectName);
            reference.AttributeName.Should().Be(attributeName);
        }
        
        [Test]
        public void New_OnlyObjectName_ShouldHaveExpectedProperties()
        {
            const string fullName = "ObjectName";

            var reference = new Reference(fullName);

            reference.Should().NotBeNull();
            reference.FullName.Should().Be(fullName);
            reference.ObjectName.Should().Be(string.Empty);
            reference.AttributeName.Should().Be(string.Empty);
        }
    }
}