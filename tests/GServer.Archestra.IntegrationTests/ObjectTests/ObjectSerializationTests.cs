using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using GCommon.Primitives;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ObjectTests
{
    [TestFixture]
    public class ObjectSerializationTests
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
        [UseReporter(typeof(DiffReporter))]
        public void Serialize_ReactorSet_ShouldPassVerification()
        {
            var target = Known.Templates.ReactorSet;
            var template = _galaxy.GetObject(target.TagName);

            var serialized = template.Serialize();

            Approvals.VerifyXml(serialized.ToString());
        }
        
        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Serialize_DrumConveyor_ShouldPassVerification()
        {
            var target = Known.Templates.DrumConveyor;
            var template = _galaxy.GetObject(target.TagName);

            var serialized = template.Serialize();

            Approvals.VerifyXml(serialized.ToString());
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