using FluentAssertions;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using GServer.Archestra.Helpers;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ObjectBuilderTests
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
        public void Build_SimpleObjectWithTagNameAndDerivedFrom_ShouldCreateAndReturnExpectedObject()
        {
            var testObject = new ArchestraObject("TestObject", Template.UserDefined);
            var builder = GalaxyBuilder.On(_galaxy).For(testObject);
            
            builder.Build();

            var result = _galaxy.GetObject("TestObject");

            result.Should().NotBeNull();
            result.TagName.Should().Be("TestObject");
            result.DerivedFromName.Should().Be(Template.UserDefined.Name);
            
            _galaxy.DeleteObject("TestObject", false);
        }
    }
}