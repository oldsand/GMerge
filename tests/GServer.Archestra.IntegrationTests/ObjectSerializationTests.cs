using FluentAssertions;
using GCommon.Primitives;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ObjectSerializationTests
    {
        private GalaxyRepository _galaxy;

        [OneTimeSetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(TestContext.GalaxyName);
            _galaxy.Login(TestContext.UserName);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
        }

        [Test]
        public void ToXml_WhenCalled_ReturnsNotNull()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObject(tagName);

            var serialized = template.Serialize();

            serialized.Should().NotBeNull();
        }

        [Test]
        public void ToXmlFromXml_SameData_ReturnsObjectWithSameProperties()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var expected = _galaxy.GetObject(tagName);

            var element = expected.Serialize();
            var result = ArchestraObject.Materialize(element);

            result.Should().BeEquivalentTo(expected);
        }
    }
}