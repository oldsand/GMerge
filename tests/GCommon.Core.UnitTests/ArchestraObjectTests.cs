using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using GCommon.Core.Enumerations;
using NUnit.Framework;

namespace GCommon.Core.UnitTests
{
    [TestFixture]
    public class ArchestraObjectTests
    {
        [Test]
        public void New_TagNameAndTemplate_ShouldNotBeNull()
        {
            var result = new ArchestraObject("TagName", Template.UserDefined);

            result.Should().NotBeNull();
        }

        [Test]
        public void New_TagNameAndTemplate_ShouldExpectedProperties()
        {
            var result = new ArchestraObject("TagName", Template.UserDefined);

            result.TagName.Should().Be("TagName");
            result.Template.Should().Be(Template.UserDefined);
            result.DerivedFromName.Should().Be(Template.UserDefined.Name);
        }

        [Test]
        public void New_FullProperties_ShouldHaveExpectedProperties()
        {
            var result = new ArchestraObject("Test", "Test.Contained", "Contained", 100, Template.Galaxy, "Derived",
                "Host", "Area", "Contained", new List<ArchestraAttribute>
                {
                    new ArchestraAttribute("Attribute1", DataType.Boolean),
                    new ArchestraAttribute("Attribute2", DataType.Integer),
                    new ArchestraAttribute("Attribute3", DataType.String)
                });

            result.TagName.Should().Be("Test");
            result.HierarchicalName.Should().Be("Test.Contained");
            result.ContainedName.Should().Be("Contained");
            result.ConfigVersion.Should().Be(100);
            result.Template.Should().Be(Template.Galaxy);
            result.DerivedFromName.Should().Be("Derived");
            result.HostName.Should().Be("Host");
            result.AreaName.Should().Be("Area");
            result.ContainerName.Should().Be("Contained");
            result.Attributes.Should().HaveCount(3);
        }

        [Test]
        public void Serialize_SimpleUserDefinedObject_ShouldNotBeNull()
        {
            var obj = new ArchestraObject("TagName", Template.UserDefined);

            var result = obj.Serialize();

            result.Should().NotBeNull();
        }

        [Test]
        public void Serialize_SimpleFakeObject_ShouldHaveExpectedAttributes()
        {
            var obj = new ArchestraObject("Test", "Test.Contained", "Contained", 100, Template.Galaxy, "Derived",
                "Host", "Area", "Contained", new List<ArchestraAttribute>
                {
                    new ArchestraAttribute("Attribute1", DataType.Boolean),
                    new ArchestraAttribute("Attribute2", DataType.Integer),
                    new ArchestraAttribute("Attribute3", DataType.String)
                });

            var result = obj.Serialize();

            result.Should().HaveAttribute(nameof(obj.TagName), obj.TagName);
            result.Should().HaveAttribute(nameof(obj.HierarchicalName), obj.HierarchicalName);
            result.Should().HaveAttribute(nameof(obj.ContainedName), obj.ContainedName);
            result.Should().HaveAttribute(nameof(obj.ConfigVersion), obj.ConfigVersion.ToString());
            result.Should().HaveAttribute(nameof(obj.Template), obj.Template.Name);
            result.Should().HaveAttribute(nameof(obj.DerivedFromName), obj.DerivedFromName);
            result.Should().HaveAttribute(nameof(obj.HostName), obj.HostName);
            result.Should().HaveAttribute(nameof(obj.AreaName), obj.AreaName);
            result.Should().HaveAttribute(nameof(obj.ContainerName), obj.ContainerName);
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Serialize_SimpleFakeObject_ShouldHaveApprovedOutput()
        {
            var obj = new ArchestraObject("Test", "Test.Contained", "Contained", 100, Template.Galaxy, "Derived",
                "Host", "Area", "Contained", new List<ArchestraAttribute>
                {
                    new ArchestraAttribute("Attribute1", DataType.Boolean),
                    new ArchestraAttribute("Attribute2", DataType.Integer),
                    new ArchestraAttribute("Attribute3", DataType.String)
                });

            var result = obj.Serialize();

            Approvals.VerifyXml(result.ToString());
        }
        
        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Serialize_SimpleUserDefinedObject_ShouldHaveApprovedOutput()
        {
            var obj = new ArchestraObject("TagName", Template.UserDefined);

            var result = obj.Serialize();

            Approvals.VerifyXml(result.ToString());
        }
    }
}