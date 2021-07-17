using System.Linq;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class RepositoryAttributeTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(Global.GalaxyName);
            _galaxy.Login(Global.UserName);
        }
        
        [Test]
        public void GetAttributes_LimitAlarmExtension_ReturnExpectedAttributes()
        {
            var template = _galaxy.GetObject("$LimitAlarms");

            var extension = template.Attributes.Where(a => a.Name.StartsWith("TestAttribute")).ToList();
            var descAttrNames = extension.Where(a => a.Name.Contains(".DescAttrName"));
            Assert.NotNull(extension);
            Assert.AreEqual(4, descAttrNames.Count());
        }
    }
}