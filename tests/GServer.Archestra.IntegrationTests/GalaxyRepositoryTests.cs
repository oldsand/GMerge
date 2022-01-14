using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using AutoFixture;
using FluentAssertions;
using GCommon.Core.Enumerations;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class GalaxyRepositoryTests
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
        public void Login_TestUser_SetsConnectionProperties()
        {
            var galaxy = new GalaxyRepository(TestConfig.GalaxyName);

            galaxy.Login(TestConfig.UserName);

            Assert.IsTrue(galaxy.Connected);
            Assert.AreEqual(TestConfig.UserName, galaxy.ConnectedUser);
        }

        [Test]
        public void Login_WindowsIdentity_ConnectsWithUserName()
        {
            var galaxy = new GalaxyRepository(TestConfig.GalaxyName);

            var user = WindowsIdentity.GetCurrent();
            galaxy.Login(user.Name);
            
            Assert.True(galaxy.Connected);
            Assert.AreEqual(user.Name, galaxy.ConnectedUser);
        }

        [Test]
        public void Logout_WhenCalled_ResetsConnectionProperties()
        {
            var galaxy = new GalaxyRepository(TestConfig.GalaxyName);

            var user = WindowsIdentity.GetCurrent();
            galaxy.Login(user.Name);
            
            Assert.True(galaxy.Connected);
            Assert.AreEqual(user.Name, galaxy.ConnectedUser);

            galaxy.Logout();

            Assert.IsFalse(galaxy.Connected);
            Assert.IsEmpty(galaxy.ConnectedUser);
        }

        [Test]
        public void UserIsAuthorized_ValidUser_ReturnsTrue()
        {
            var result = _galaxy.UserIsAuthorized(TestConfig.UserName);
            Assert.True(result);
        }
        
        [Test]
        public void UserIsAuthorized_InvalidUser_ReturnsFalse()
        {
            var fixture = new Fixture();
            var result = _galaxy.UserIsAuthorized(fixture.Create<string>());
            Assert.False(result);
        }

        [Test]
        public void GetObject_ValidTagName_ReturnsCorrectTemplates()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var result = _galaxy.GetObject(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
        }

        [Test]
        public void GetObject_InvalidTemplate_ReturnsNull()
        {
            var fixture = new Fixture();
            var template = _galaxy.GetObject(fixture.Create<string>());

            Assert.IsNull(template);
        }

        [Test]
        public void GetObject_TagNameWithMultipleObjects_ReturnsNotNull()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var result = _galaxy.GetObject(tagName);

            result.Should().NotBeNull();
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
            var result = _galaxy.GetGraphic(tagName);
            
            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.TagName);
        }
    }
}