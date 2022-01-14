using System.Linq;
using ArchestrA.GRAccess;
using FluentAssertions;
using GCommon.Core;
using GCommon.Core.Enumerations;
using GCommon.Core.Structs;
using GServer.Archestra.Extensions;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ExtensionTests
{
    [TestFixture]
    public class AttributeExtensionTests
    {
        private static IGalaxy _galaxy;
        private GRAccessAppClass _grAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _grAccess = new GRAccessAppClass();
            _galaxy = _grAccess.QueryGalaxies()[TestConfig.GalaxyName];
            _galaxy.Login("DefaultUser", string.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
            _galaxy = null;
        }

        [Test]
        public void GetValue_KnownBoolean_ReturnsExpectedFalse()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);

            var result = template.Attributes["Auto"].GetValue<bool>();

            result.Should().BeFalse();
        }
        
        [Test]
        public void GetValue_KnownInteger_ShouldReturnExpectedValue()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);

            var result = template.GetAttribute("BatchNum").GetValue<int>();

            result.Should().Be(0);
        }
        
        [Test]
        public void GetValue_KnownReference_ShouldReturnExpectedValue()
        {
            var expected = Reference.Empty;
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);

            var result = template.GetAttribute("BatchNum.InputSource").GetValue<Reference>();

            result.Should().Be(expected);
        }
        
        [Test]
        public void GetValue_KnownCustomStruct_ShouldReturnExpectedValue()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);

            var result = template.GetAttribute("Concentrate._VisualElementDefinition").GetValue<Blob>();

            result.Should().NotBeNull();
            result.Data.Should().NotBeEmpty();
        }
        
        [Test]
        public void SetValue_KnownBooleanToTrue_ReturnsTrue()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);
            template.CheckOut();

            template.Attributes["Auto"].SetValue(true);

            var result = template.Attributes["Auto"].GetValue<bool>();
            result.Should().BeTrue();

            template.ForceClose();
            template.IsCheckedOut().Should().BeFalse();

            result = template.Attributes["Auto"].GetValue<bool>();
            result.Should().BeFalse();
        }
        
        [Test]
        public void SetValue_StructTesterVisualDefinition_ReturnsTrue()
        {
            var template = _galaxy.GetObjectByName("$StructTester");
            template.CheckOut();

            var attribute = template.GetAttribute("TestGraphic._VisualElementDefinition");
            var primitive = attribute.MapAttribute();
            attribute.SetLocked(MxPropertyLockedEnum.MxUnLocked);
            template.Save();
            
            attribute.SetValue(Blob.Empty);
            template.Save();

            var changed = template.GetAttribute("TestGraphic._VisualElementDefinition").GetValue<Blob>();
            //changed.Should().Be(Blob.Empty());

            template.ForceClose();
            template.IsCheckedOut().Should().BeFalse();

            var result = template.GetAttribute("TestGraphic._VisualElementDefinition").GetValue<Blob>();
            result.Data.Should().NotBeEmpty();
        }
        
        [Test]
        public void ByDataType_KnownTemplateWithBooleans_ReturnsOnlyBooleanAttributes()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);
            
            var attributes = template.Attributes.ByDataType(DataType.Boolean).ToList();

            attributes.Should().NotBeEmpty();
            attributes.All(x => x.DataType == DataType.Boolean).Should().BeTrue();
        }

        [Test]
        public void ByNameContains_TestTemplate_AttributeDataMatchesExpected()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);
            
            var attributes = template.Attributes.ByNameContains("Batch").ToList();
            
            attributes.Should().NotBeEmpty();
            attributes.All(x => x.Name.Contains("Batch")).Should().BeTrue();
        }
    }
}