using System.Linq;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ArchestraObjectTests
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
        public void GetAttributes_SiteData_ReturnSomeExpectedAttributes()
        {
            var siteData = _galaxy.GetObject("$Site_Data");

            var attributes = siteData.Attributes.ToList();

            Assert.IsTrue(attributes.Any(a => a.Name == "SiteName"));
            Assert.AreEqual("Generic", attributes.SingleOrDefault(a => a.Name == "SiteName")?.Value);
            Assert.IsTrue(attributes.Any(a => a.Name == "CompanyName"));
            Assert.AreEqual("Energy Transfer", attributes.SingleOrDefault(a => a.Name == "CompanyName")?.Value);
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
        [TestCase("BoolTest")]
        [TestCase("DoubleTest")]
        [TestCase("FloatTest")]
        public void GetAttributes_TestTemplate_AttributeDataMatchesExpected(string attributeName)
        {
            var template = _galaxy.GetObject("$Test_Template");
            var attribute = template.Attributes.SingleOrDefault(a => a.Name == attributeName);
            Assert.NotNull(attribute);
            Assert.NotNull(attribute.Name);
            Assert.NotNull(attribute.DataType);
            Assert.NotNull(attribute.Category);
            Assert.NotNull(attribute.Security);
            Assert.NotNull(attribute.Locked);
        }
    }
}