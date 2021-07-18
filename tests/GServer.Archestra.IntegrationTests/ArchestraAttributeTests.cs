using System.Linq;
using GCommon.Primitives;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ArchestraAttributeTests
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
        public void GetAttributes_LimitAlarmExtension_ReturnExpectedAttributes()
        {
            var template = _galaxy.GetObject("$LimitAlarms");

            var extension = template.Attributes.Where(a => a.Name.StartsWith("TestAttribute")).ToList();
            var descAttrNames = extension.Where(a => a.Name.Contains(".DescAttrName"));
            Assert.NotNull(extension);
            Assert.AreEqual(4, descAttrNames.Count());
        }
        
        [Test]
        [TestCase("$Test_Template_001")]
        public void GetByCategoryAttributes_ValidTagName_ReturnsExpected(string tagName)
        {
            var template = _galaxy.GetObject(tagName);

            var attributes = template.Attributes.ToList();

            var calculated = attributes.Where(a => a.Category == AttributeCategory.Calculated);
            var writeable = attributes.Where(a => a.Category == AttributeCategory.Writeable_USC_Lockable);

            Assert.True(calculated.Any(a => a.Name == "Calculated"));
            Assert.True(writeable.Any(a => a.Name == "UserWriteable"));
        }
    }
}