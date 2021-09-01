using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using GCommon.Primitives.Enumerations;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests
{
    [TestFixture]
    public class ArchestraObjectTests
    {
        [Test]
        public void Construction_WhenCalled_ShouldNotBeNull()
        {
            var result = new ArchestraObject();

            result.Should().NotBeNull();
        }

        [Test]
        public void Construction_WithParameters_PropertiesShouldBeExpected()
        {
            var result = new ArchestraObject
            {
                TagName = "Test",
                ContainedName = "Contained",
                HierarchicalName = "Test.Contained",
                Category = ObjectCategory.Galaxy,
                ConfigVersion = 100,
                DerivedFromName = "Derived",
                BasedOnName = "BasedOn"
                
            };

            result.TagName.Should().Be("Test");
            result.ContainedName.Should().Be("Contained");
            result.HierarchicalName.Should().Be("Test.Contained");
            result.Category.Should().Be(ObjectCategory.Galaxy);
            result.ConfigVersion.Should().Be(100);
            result.DerivedFromName.Should().Be("Derived");
            result.BasedOnName.Should().Be("BasedOn");
        }
        
        [Test]
        public void Serialize_WhenCalled_ShouldNotBeNull()
        {
            var obj = new ArchestraObject();

            var result = obj.Serialize();

            result.Should().NotBeNull();
        }

        [Test]
        public void Serialize_WhenCalled_ShouldHaveExpectedAttributes()
        {
            var obj = new ArchestraObject
            {
                TagName = "Test",
                ContainedName = "Contained",
                HierarchicalName = "Test.Contained",
                Category = ObjectCategory.Galaxy,
                ConfigVersion = 100,
                DerivedFromName = "Derived",
                BasedOnName = "BasedOn",
                HostName = "Host",
                AreaName = "Area",
                ContainerName = "Container",
                Attributes = new List<ArchestraAttribute>
                {
                    new ArchestraAttribute("Attribute1", DataType.Boolean),
                    new ArchestraAttribute("Attribute2", DataType.Integer),
                    new ArchestraAttribute("Attribute3", DataType.String)
                }
            };

            var result = obj.Serialize();

            result.Should().HaveAttribute(nameof(obj.TagName), obj.TagName);
            result.Should().HaveAttribute(nameof(obj.ContainedName), obj.ContainedName);
            result.Should().HaveAttribute(nameof(obj.HierarchicalName), obj.HierarchicalName);
            result.Should().HaveAttribute(nameof(obj.Category), obj.Category.ToString());
            result.Should().HaveAttribute(nameof(obj.ConfigVersion), obj.ConfigVersion.ToString());
            result.Should().HaveAttribute(nameof(obj.DerivedFromName), obj.DerivedFromName);
            result.Should().HaveAttribute(nameof(obj.BasedOnName), obj.BasedOnName);
            result.Should().HaveAttribute(nameof(obj.HostName), obj.HostName);
            result.Should().HaveAttribute(nameof(obj.AreaName), obj.AreaName);
            result.Should().HaveAttribute(nameof(obj.ContainerName), obj.ContainerName);
        }
        
        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Serialize_WhenCalled_ShouldHaveApprovedOutput()
        {
            var obj = new ArchestraObject
            {
                TagName = "Test",
                ContainedName = "Contained",
                HierarchicalName = "Test.Contained",
                Category = ObjectCategory.Galaxy,
                ConfigVersion = 100,
                DerivedFromName = "Derived",
                BasedOnName = "BasedOn",
                HostName = "Host",
                AreaName = "Area",
                ContainerName = "Container",
                Attributes = new List<ArchestraAttribute>
                {
                    new ArchestraAttribute("Attribute1", DataType.Boolean),
                    new ArchestraAttribute("Attribute2", DataType.Integer),
                    new ArchestraAttribute("Attribute3", DataType.String)
                }
            };

            var result = obj.Serialize();

            Approvals.VerifyXml(result.ToString());
        }
    }
}