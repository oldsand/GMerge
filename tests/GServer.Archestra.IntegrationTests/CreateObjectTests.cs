using GCommon.Primitives;
using NUnit.Framework;
using TestContext = GServer.Archestra.IntegrationTests.TestContext;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ObjectEditorTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(TestContext.GalaxyName);
            _galaxy.Login(TestContext.UserName);
        }

        [Test]
        public void CreateObject_LimitAlarms_ReturnsCorrectConfiguration()
        {
            //Get the known template
            var template = (ArchestraObject) _galaxy.GetObject("$LimitAlarms");
            Assert.NotNull(template);

            //Serialize data to in memory object
            var data = template.Serialize();
            var galaxyObject = ArchestraObject.Materialize(data);
            Assert.NotNull(galaxyObject);

            //Delete the current template
            _galaxy.DeleteObject("$LimitAlarms", true);
            var shouldNotExist = _galaxy.GetObject("$LimitAlarms");
            Assert.IsNull(shouldNotExist);

            //rebuild the template
            _galaxy.CreateObject(galaxyObject);
            var shouldExist = _galaxy.GetObject("$LimitAlarms");
            Assert.NotNull(shouldExist);
        }

        [Test]
        public void CreateObject_AlarmExtension_ReturnsCorrectConfiguration()
        {
            //Get the known template
            var template = (ArchestraObject) _galaxy.GetObject("$AlarmExtension");
            Assert.NotNull(template);

            //Serialize data to in memory object
            var data = template.Serialize();
            var galaxyObject = ArchestraObject.Materialize(data);
            Assert.NotNull(galaxyObject);

            //Delete the current template
            _galaxy.DeleteObject("$AlarmExtension", false);
            var shouldNotExist = _galaxy.GetObject("$AlarmExtension");
            Assert.IsNull(shouldNotExist);

            //rebuild the template
            _galaxy.CreateObject(galaxyObject);
            var shouldExist = _galaxy.GetObject("$AlarmExtension");
            Assert.NotNull(shouldExist);
        }

        [Test]
        public void CreateObject_RocAlarms_ReturnsCorrectConfiguration()
        {
            //Get the known template
            var template = (ArchestraObject) _galaxy.GetObject("$ROCAlarms");
            Assert.NotNull(template);

            //Serialize data to in memory object
            var data = template.Serialize();
            var galaxyObject = ArchestraObject.Materialize(data);
            Assert.NotNull(galaxyObject);

            //Delete the current template
            _galaxy.DeleteObject("$ROCAlarms", false);
            var shouldNotExist = _galaxy.GetObject("$ROCAlarms");
            Assert.IsNull(shouldNotExist);

            //rebuild the template
            _galaxy.CreateObject(galaxyObject);
            var shouldExist = _galaxy.GetObject("$ROCAlarms");
            Assert.NotNull(shouldExist);
        }

        [Test]
        public void CreateObject_IOExtensions_ReturnsCorrectConfiguration()
        {
            //Get the known template
            var template = (ArchestraObject) _galaxy.GetObject("$IOExtensions");
            Assert.NotNull(template);

            //Serialize data to in memory object
            var data = template.Serialize();
            var galaxyObject = ArchestraObject.Materialize(data);
            Assert.NotNull(galaxyObject);

            //Delete the current template
            _galaxy.DeleteObject("$IOExtensions", false);
            var shouldNotExist = _galaxy.GetObject("$IOExtensions");
            Assert.IsNull(shouldNotExist);

            //rebuild the template
            _galaxy.CreateObject(galaxyObject);
            var shouldExist = _galaxy.GetObject("$IOExtensions");
            Assert.NotNull(shouldExist);
        }

        [Test]
        public void CreateObject_ScriptExtension_ReturnsCorrectConfiguration()
        {
            //Get the known template
            var template = (ArchestraObject) _galaxy.GetObject("$ScriptExtension");
            Assert.NotNull(template);

            //Serialize data to in memory object
            var data = template.Serialize();
            var galaxyObject = ArchestraObject.Materialize(data);
            Assert.NotNull(galaxyObject);

            //Delete the current template
            _galaxy.DeleteObject("$ScriptExtension", false);
            var shouldNotExist = _galaxy.GetObject("$ScriptExtension");
            Assert.IsNull(shouldNotExist);

            //rebuild the template
            _galaxy.CreateObject(galaxyObject);
            var shouldExist = _galaxy.GetObject("$ScriptExtension");
            Assert.NotNull(shouldExist);
        }

        [Test]
        public void CreateObject_SiteData_ReturnsCorrectConfiguration()
        {
            //Get the known template
            var template = (ArchestraObject) _galaxy.GetObject("$Site_Data");
            Assert.NotNull(template);

            //Serialize data to in memory object
            var data = template.Serialize();
            var galaxyObject = ArchestraObject.Materialize(data);
            Assert.NotNull(galaxyObject);

            //Delete the current template
            _galaxy.DeleteObject("$Site_Data", false);
            var shouldNotExist = _galaxy.GetObject("$Site_Data");
            Assert.IsNull(shouldNotExist);

            //rebuild the template
            _galaxy.CreateObject(galaxyObject);
            var shouldExist = _galaxy.GetObject("$Site_Data");
            Assert.NotNull(shouldExist);
        }
    }
}