using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GCommon.Core.Enumerations;
using GCommon.Core.Helpers;
using NUnit.Framework;

namespace GCommon.Core.UnitTests.HelperTests
{
    [TestFixture]
    public class CommandDataInfoTests
    {
        [Test]
        public void New_ValidAttribute_ShouldNotBeNull()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            
            var info = new CommandDataInfo(attribute);

            info.Should().NotBeNull();
        }
        
        [Test]
        public void New_ValidAttribute_ShouldHaveDefaultConfig()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            
            var info = new CommandDataInfo(attribute);

            info.Config.Should().Be(CommandDataInfo.DefaultValue);
        }
        
        [Test]
        public void New_Null_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new CommandDataInfo(null)).Should().Throw<ArgumentNullException>();
        }
        
        [Test]
        public void New_InvalidAttributeName_ShouldThrowInvalidOperationException()
        {
            var attribute = new ArchestraAttribute("Invalid", DataType.String);
            FluentActions.Invoking(() => new CommandDataInfo(attribute)).Should().Throw<InvalidOperationException>();
        }
        
        [Test]
        public void New_InvalidAttributeType_ShouldThrowInvalidOperationException()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.Integer);
            FluentActions.Invoking(() => new CommandDataInfo(attribute)).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Add_ValidAttribute_ConfigShouldContainAttribute()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);

            var expected = new ArchestraAttribute("Test", DataType.Boolean);
            info.Add(expected.GenerateCmdData());
            
            info.Config.Should().Contain(expected.GenerateCmdData().ToString());
        }
        
        [Test]
        public void Add_ValidAttribute_AttributeValueShouldBeConfig()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);

            info.Add(new ArchestraAttribute("Test", DataType.Boolean).GenerateCmdData());
            
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Add_NonBooleanAttributes_ShouldHaveDefaultValue()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);

            var attributes = new List<ArchestraAttribute>()
            {
                new ArchestraAttribute("Integer", DataType.Integer),
                new ArchestraAttribute("Double", DataType.Double),
                new ArchestraAttribute("Float", DataType.Float),
                new ArchestraAttribute("String", DataType.String),
                new ArchestraAttribute("ReferenceType", DataType.ReferenceType),
            }.Select(x => x.GenerateCmdData());
            
            info.Add(attributes);
            
            info.Config.Should().Contain(CommandDataInfo.DefaultValue);
        }
        
        [Test]
        public void Add_MultipleDifferentValidAttributes_ConfigShouldContainAllAttributes()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);
            var first = new ArchestraAttribute("First", DataType.Boolean);
            var second = new ArchestraAttribute("Second", DataType.Boolean);
            var third = new ArchestraAttribute("Third", DataType.Boolean);
            
            info.Add(first.GenerateCmdData());
            info.Add(second.GenerateCmdData());
            info.Add(third.GenerateCmdData());

            info.Config.Should().Contain(first.GenerateCmdData().ToString());
            info.Config.Should().Contain(second.GenerateCmdData().ToString());
            info.Config.Should().Contain(third.GenerateCmdData().ToString());
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Add_MultipleSameValidAttributes_ShouldOnlyContainFirst()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);
            
            var first = new ArchestraAttribute("Test", DataType.Boolean);
            var second = new ArchestraAttribute("Test", DataType.Boolean);
            var third = new ArchestraAttribute("Test", DataType.Boolean);
            
            info.Add(first.GenerateCmdData());
            info.Add(second.GenerateCmdData());
            info.Add(third.GenerateCmdData());

            info.XConfig.Descendants("Attribute").Count().Should().Be(1);
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Remove_ValidAttribute_ConfigShouldNotContainAttribute()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Boolean);
            info.Add(item.GenerateCmdData());

            info.Remove(item.GenerateCmdData());
            
            info.Config.Should().NotContain(item.GenerateCmdData().ToString());
        }
        
        [Test]
        public void Remove_ValidAttribute_AttributeValueShouldBeConfig()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Boolean);
            info.Add(item.GenerateCmdData());

            info.Remove(item.GenerateCmdData());
            
            attribute.Value.Should().Be(info.Config);
        }
        
        [Test]
        public void Replace_ValidAttribute_ConfigShouldContainNewAttribute()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Boolean);
            info.Add(item.GenerateCmdData());

            var expected = new ArchestraAttribute("Test", DataType.Boolean);
            info.Replace(expected.GenerateCmdData());
            
            info.Config.Should().Contain(expected.GenerateCmdData().ToString());
        }
        
        [Test]
        public void Replace_ValidAttribute_AttributeValueShouldBeConfig()
        {
            var attribute = new ArchestraAttribute(CommandDataInfo.AttributeName, DataType.String);
            var info = new CommandDataInfo(attribute);
            var item = new ArchestraAttribute("Test", DataType.Boolean);
            info.Add(item.GenerateCmdData());

            var expected = new ArchestraAttribute("Test", DataType.Boolean);
            info.Replace(expected.GenerateCmdData());
            
            attribute.Value.Should().Be(info.Config);
        }
    }
}