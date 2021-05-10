using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Testing;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class ObjectSerializationTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(Settings.CurrentTestGalaxy);
            _galaxy.Login(Settings.CurrentTestUser);
        }
        
        [Test]
        [TestCase("$Test_Template")]
        public void ToXml_WhenCalled_ReturnsNotNull(string tagName)
        {
            var template = (GalaxyObject) _galaxy.GetObject(tagName);

            var serialized = template.ToXml();
            
            Assert.NotNull(serialized);
        }
        
        [Test]
        [TestCase("$Test_Template")]
        public void ToXmlFromXml_SameData_ReturnsObjectWithSameProperties(string tagName)
        {
            var template = (GalaxyObject) _galaxy.GetObject(tagName);

            var xml = template.ToXml();
            var galaxyObject = new GalaxyObject().FromXml(xml);
            
            Assert.AreEqual(template.TagName, galaxyObject.TagName);
        }
    }
}