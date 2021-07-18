using GServer.Archestra.Entities;
using NUnit.Framework;

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
        public void ToXml_WhenCalled_ReturnsNotNull()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObject(tagName);

            var serialized = template.ToXml();
            
            Assert.NotNull(serialized);
        }
        
        [Test]
        public void ToXmlFromXml_SameData_ReturnsObjectWithSameProperties()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObject(tagName);

            var xml = template.ToXml();
            var galaxyObject = new ArchestraObject().FromXml(xml);
            
            Assert.AreEqual(template.TagName, galaxyObject.TagName);
        }
    }
}