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
            result.BasedOnName.Should().Be(Template.UserDefined.Name);
            result.DerivedFromName.Should().Be(Template.UserDefined.Name);
        }

        [Test]
        public void New_FullProperties_ShouldHaveExpectedProperties()
        {
            var result = new ArchestraObject("Test", "Contained", "Test.Contained", ObjectCategory.Galaxy, 100,
                "Derived", "BasedOn", "Host", "Area", "Contained", Template.Galaxy.GetAttributes());

            result.TagName.Should().Be("Test");
            result.ContainedName.Should().Be("Contained");
            result.HierarchicalName.Should().Be("Test.Contained");
            result.Category.Should().Be(ObjectCategory.Galaxy);
            result.ConfigVersion.Should().Be(100);
            result.DerivedFromName.Should().Be("Derived");
            result.BasedOnName.Should().Be("BasedOn");
            result.HostName.Should().Be("Host");
            result.AreaName.Should().Be("Area");
            result.ContainerName.Should().Be("Contained");
            result.Attributes.Should().BeEquivalentTo(Template.Galaxy.GetAttributes());
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
            var obj = new ArchestraObject("Test", "Contained", "Test.Contained", ObjectCategory.Galaxy, 100,
                "Derived", "BasedOn", "Host", "Area", "Contained",
                new List<ArchestraAttribute>
                {
                    new ArchestraAttribute("Attribute1", DataType.Boolean),
                    new ArchestraAttribute("Attribute2", DataType.Integer),
                    new ArchestraAttribute("Attribute3", DataType.String)
                });

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
        public void Serialize_SimpleFakeObject_ShouldHaveApprovedOutput()
        {
            var obj = new ArchestraObject("Test", "Contained", "Test.Contained", ObjectCategory.Galaxy, 100,
                "Derived", "BasedOn", "Host", "Area", "Contained",
                new List<ArchestraAttribute>
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