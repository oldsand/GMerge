using System.IO;
using FluentAssertions;
using GCommon.Core.Utilities;
using GServer.Archestra.IntegrationTests.Base;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyFileManagerTests
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
        public void ExportGraphic_WhenCalled_FileShouldExist()
        {
            using var temp = new TempDirectory();
            var fileName = Path.Combine(temp.FullName, "React.xml");
            var manager = new GalaxyFileManager(_galaxy);
            
            manager.ExportGraphic(Known.Symbols.React, fileName);

            File.Exists(fileName).Should().BeTrue();
        }
    }
}