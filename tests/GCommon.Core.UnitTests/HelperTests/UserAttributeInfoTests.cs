using System;
using FluentAssertions;
using GCommon.Core.Enumerations;
using GCommon.Core.Helpers;
using NUnit.Framework;

namespace GCommon.Core.UnitTests.HelperTests
{
    [TestFixture]
    public class UserAttributeInfoTests
    {
        [Test]
        public void New_ValidAttribute_ShouldNotBeNull()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            
            var info = new UserAttributeInfo(attribute);

            info.Should().NotBeNull();
        }
        
        [Test]
        public void New_ValidAttribute_ShouldHaveDefaultConfig()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            
            var info = new UserAttributeInfo(attribute);

            info.Config.Should().Be("<UDAInfo></UDAInfo>");
        }
        
        [Test]
        public void New_Null_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new UserAttributeInfo(null)).Should().Throw<ArgumentNullException>();
        }
        
        [Test]
        public void New_InvalidAttributeName_ShouldThrowInvalidOperationException()
        {
            var attribute = new ArchestraAttribute("UDA", DataType.String);
            FluentActions.Invoking(() => new UserAttributeInfo(attribute)).Should().Throw<InvalidOperationException>();
        }
        
        [Test]
        public void New_InvalidAttributeType_ShouldThrowInvalidOperationException()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.Integer);
            FluentActions.Invoking(() => new UserAttributeInfo(attribute)).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Add_ValidAttribute_ConfigShouldContainAttribute()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);

            var expected = new ArchestraAttribute("Test", DataType.Double);
            info.Add(expected.GenerateInfo());
            
            info.Config.Should().Contain(expected.GenerateInfo().ToString());
        }
        
        [Test]
        public void Add_ValidAttribute_AttributeValueShouldBeConfig()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);

            info.Add(new ArchestraAttribute("Test", DataType.Double).GenerateInfo());
            
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Add_MultipleDifferentValidAttributes_ConfigShouldContainAllAttributes()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);
            var first = new ArchestraAttribute("Double", DataType.Double);
            var second = new ArchestraAttribute("Boolean", DataType.Boolean);
            var third = new ArchestraAttribute("String", DataType.String);
            
            info.Add(first.GenerateInfo());
            info.Add(second.GenerateInfo());
            info.Add(third.GenerateInfo());

            info.Config.Should().Contain(first.GenerateInfo().ToString());
            info.Config.Should().Contain(second.GenerateInfo().ToString());
            info.Config.Should().Contain(third.GenerateInfo().ToString());
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Add_MultipleSameValidAttributes_ShouldOnlyContainFirst()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);
            
            var first = new ArchestraAttribute("Test", DataType.Double);
            var second = new ArchestraAttribute("Test", DataType.Boolean);
            var third = new ArchestraAttribute("Test", DataType.String);
            
            info.Add(first.GenerateInfo());
            info.Add(second.GenerateInfo());
            info.Add(third.GenerateInfo());
            
            info.Config.Should().Contain(first.GenerateInfo().ToString());
            info.Config.Should().NotContain(second.GenerateInfo().ToString());
            info.Config.Should().NotContain(third.GenerateInfo().ToString());
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Remove_ValidAttribute_ConfigShouldNotContainAttribute()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Double);
            info.Add(item.GenerateInfo());

            info.Remove(item.GenerateInfo());
            
            info.Config.Should().NotContain(item.GenerateInfo().ToString());
        }
        
        [Test]
        public void Remove_ValidAttribute_AttributeValueShouldBeConfig()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Double);
            info.Add(item.GenerateInfo());

            info.Remove(item.GenerateInfo());
            
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Replace_ValidAttribute_ConfigShouldContainNewAttribute()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Double);
            info.Add(item.GenerateInfo());

            var expected = new ArchestraAttribute("Test", DataType.Float);
            info.Replace(expected.GenerateInfo());
            
            info.Config.Should().Contain(expected.GenerateInfo().ToString());
        }
        
        [Test]
        public void Replace_ValidAttribute_AttributeValueShouldBeConfig()
        {
            var attribute = new ArchestraAttribute("UDAs", DataType.String);
            var info = new UserAttributeInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Double);
            info.Add(item.GenerateInfo());

            var expected = new ArchestraAttribute("Test", DataType.Float);
            info.Replace(expected.GenerateInfo());
            
            attribute.Value.Should().Be(info.Config);
        }
    }
}