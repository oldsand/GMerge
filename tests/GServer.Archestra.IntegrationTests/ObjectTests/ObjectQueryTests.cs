using System.Linq;
using FluentAssertions;
using GServer.Archestra.IntegrationTests.Base;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ObjectTests
{
    [TestFixture]
    public class ObjectQueryTests
    {
        private GalaxyRepository _galaxy;

        [OneTimeSetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(TestConfig.GalaxyName);
            _galaxy.Login(TestConfig.UserName);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
        }

        [Test]
        public void GetPredefinedUdaAndExtensionAttributes()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var galaxyObject = _galaxy.GetObject(tagName);

            var attributes = galaxyObject.Attributes.ToList();
            var inheritedExtensions = attributes.SingleOrDefault(a => a.Name == "_InheritedExtensions");
            var inheritedUda = attributes.SingleOrDefault(a => a.Name == "_InheritedUDAs");
            var extensions = attributes.SingleOrDefault(a => a.Name == "Extensions");
            var uda = attributes.SingleOrDefault(a => a.Name == "UDAs");

            Assert.NotNull(inheritedExtensions);
            Assert.NotNull(inheritedUda);
            Assert.NotNull(extensions);
            Assert.NotNull(uda);
        }

        [Test]
        public void GetAttribute_FromKnownObject_ShouldHaveKnownAttributeValues()
        {
            var target = Known.Templates.ReactorSet;
            var attribute = target.Attributes.First();
            
            var template = _galaxy.GetObject(target.TagName);
            var result = template.Attributes.Single(a => a.Name == attribute.Name);

            result.Should().NotBeNull();
            result.Name.Should().Be(attribute.Name);
            result.DataType.Should().Be(attribute.DataType);
            result.Category.Should().Be(attribute.Category);
            result.Security.Should().Be(attribute.Security);
            result.Locked.Should().Be(attribute.Locked);
        }
    }
}