using GServer.Archestra.Entities;
using NUnit.Framework;
using TestContext = GServer.Archestra.IntegrationTests.TestContext;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ObjectSerializationTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(TestContext.GalaxyName);
            _galaxy.Login(TestContext.UserName);
        }
        
        [Test]
        [TestCase("$Test_Template")]
        public void ToXml_WhenCalled_ReturnsNotNull(string tagName)
        {
            var template = (ArchestraObject) _galaxy.GetObject(tagName);

            var serialized = template.ToXml();
            
            Assert.NotNull(serialized);
        }
        
        [Test]
        [TestCase("$Test_Template")]
        public void ToXmlFromXml_SameData_ReturnsObjectWithSameProperties(string tagName)
        {
            var template = (ArchestraObject) _galaxy.GetObject(tagName);

            var xml = template.ToXml();
            var galaxyObject = new ArchestraObject().FromXml(xml);
            
            Assert.AreEqual(template.TagName, galaxyObject.TagName);
        }
    }
}