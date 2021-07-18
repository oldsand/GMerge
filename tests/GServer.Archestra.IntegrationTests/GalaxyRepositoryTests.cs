using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using NUnit.Framework;
using TestContext = GServer.Archestra.IntegrationTests.TestContext;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyRepositoryTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(TestContext.GalaxyName);
            _galaxy.Login(TestContext.UserName);
        }

        [Test]
        [TestCase("tnunnink")]
        [TestCase("")]
        [TestCase("admin")]
        public void Login_WhenCalled_SetsConnectionProperties(string userName)
        {
            var galaxy = new GalaxyRepository(TestContext.GalaxyName);

            galaxy.Login(userName);

            Assert.IsTrue(galaxy.Connected);
            Assert.AreEqual(userName, galaxy.ConnectedUser);
        }

        [Test]
        public void Login_WindowsIdentity_ConnectsWithUserName()
        {
            var galaxy = new GalaxyRepository(TestContext.GalaxyName);

            var user = WindowsIdentity.GetCurrent();
            galaxy.Login(user.Name);
            
            Assert.True(galaxy.Connected);
            Assert.AreEqual(user.Name, galaxy.ConnectedUser);
        }

        [Test]
        public void Logout_WhenCalled_ResetsConnectionProperties()
        {
            var galaxy = new GalaxyRepository("ButaneDev2014");
            galaxy.Login("admin");
            Assert.IsTrue(galaxy.Connected);
            Assert.AreEqual("admin", galaxy.ConnectedUser);

            galaxy.Logout();

            Assert.IsFalse(galaxy.Connected);
            Assert.IsEmpty(galaxy.ConnectedUser);
        }

        [Test]
        public void UserIsAuthorized_ValidUser_ReturnsTrue()
        {
            var result = _galaxy.UserIsAuthorized(@"ENE\tnunnink");
            Assert.True(result);
        }
        
        [Test]
        public void UserIsAuthorized_InvalidUser_ReturnsTrue()
        {
            var result = _galaxy.UserIsAuthorized(@"ENE\FakeUser");
            Assert.False(result);
        }

        [Test]
        [TestCase("$Site_Data")]
        [TestCase("$Test_Template")]
        [TestCase("$Extensions")]
        public void GetObject_ValidTagName_ReturnsCorrectTemplates(string tagName)
        {
            var template = _galaxy.GetObject(tagName);

            Assert.NotNull(template);
            Assert.AreEqual(tagName, template.TagName);
        }

        [Test]
        [TestCase("$SomeTemplate")]
        [TestCase("$FakeTemplate")]
        public void GetObject_InvalidTemplate_ReturnsNull(string tagName)
        {
            var template = _galaxy.GetObject(tagName);

            Assert.IsNull(template);
        }

        [Test]
        [TestCase("$EthFLP01.EthFLP01_SmplPmp.EthFLP01_SmplPmp_FTA")]
        public void GetObject_TagNameWithMultipleObjects_ReturnsNotNull(string tagName)
        {
            var result = _galaxy.GetObject(tagName);

            Assert.NotNull(result);
        }

        [Test]
        public void GetObjects_ValidTagName_ReturnsCorrectTemplates()
        {
            var tagList = new List<string> {"$Site_Data", "$Test_Template", "$TestObjects"};
            var templates = _galaxy.GetObjects(tagList).ToList();

            Assert.IsNotEmpty(templates);
            Assert.That(templates, Has.Count.EqualTo(3));
            Assert.IsTrue(templates.Any(t => t.TagName == "$Site_Data"));
            Assert.IsTrue(templates.Any(t => t.TagName == "$Test_Template"));
            Assert.IsTrue(templates.Any(t => t.TagName == "$TestObjects"));
        }

        [Test]
        [TestCase("Test_Template_Instance")]
        [TestCase("SUN_GEN_Site_Data")]
        [TestCase("SUN_GEN_Site_Area")]
        [TestCase("FileCopy")]
        public void GetObjects_ValidTagName_ReturnsExpectedInstance(string tagName)
        {
            var instance = _galaxy.GetObject(tagName);

            Assert.NotNull(instance);
            Assert.AreEqual(tagName, instance.TagName);
        }

        [Test]
        [TestCase("FileCopy")]
        [TestCase("FormatString")]
        [TestCase("DatabaseAccess")]
        [TestCase("SearchBar")]
        [TestCase("_PIDTuning_DataCollector")]
        [TestCase("Symbol_020")]
        public void GetSymbol_ValidaTagName_ReturnsExpectedInstance(string tagName)
        {
            var instance = _galaxy.GetGraphic(tagName);
            
            Assert.NotNull(instance);
            Assert.AreEqual(tagName, instance.TagName);
        }

        [Test]
        public void ExportPkg_ValidObjects_CreatesFile()
        {
            _galaxy.ExportPkg("$Test_Template",
                @"C:\Users\tnunnink\Documents\Export\GalaxyAccess\TestExport.aaPKG");

            FileAssert.Exists(@"C:\Users\tnunnink\Documents\Export\GalaxyAccess\TestExport.aaPKG");
        }

        [Test]
        public void ExportSymbol_ValidSymbol_CreatesFile()
        {
            _galaxy.ExportGraphic("TestSymbol",
                @"C:\Users\tnunnink\Documents\Export\GalaxyAccess\TestSymbol.xml");
            
            FileAssert.Exists(@"C:\Users\tnunnink\Documents\Export\GalaxyAccess\TestSymbol.xml");
        }

        [Test]
        public void ImportSymbol_ValidSymbol_LoadsSymbol()
        {
            _galaxy.ImportGraphic(@"C:\Users\tnunnink\Documents\Export\GalaxyAccess\TestSymbol.xml", "NewTestSymbol", false);
            
            Assert.Pass();
        }
        
        [Test]
        public void ImportSymbol_Overwrite_LoadsSymbol()
        {
            _galaxy.ImportGraphic(@"C:\Users\tnunnink\Documents\Export\GalaxyAccess\TestSymbol.xml", "TestSymbol", true);
            
            Assert.Pass();
        }

        [Test]
        public void GetSymbol_ValidSymbol_ReturnsGalaxySymbol()
        {
            var symbol = _galaxy.GetGraphic("DatabaseAccess");
            
            Assert.NotNull(symbol);
            Assert.True(symbol.CustomProperties.Any(p => p.Name == "Execute"));
        }
    }
}