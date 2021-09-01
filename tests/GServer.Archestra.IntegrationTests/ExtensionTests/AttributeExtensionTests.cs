using System.Linq;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using GServer.Archestra.Extensions;
using GServer.Archestra.IntegrationTests.Base;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ExtensionTests
{
    [TestFixture]
    public class AttributeExtensionTests
    {
        private static IGalaxy _galaxy;
        private GRAccessAppClass _grAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _grAccess = new GRAccessAppClass();
            _galaxy = _grAccess.QueryGalaxies()[TestConfig.GalaxyName];
            _galaxy.Login("DefaultUser", string.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
            _galaxy = null;
        }

        [Test]
        public void GetValue_KnownBoolean_ReturnsExpectedFalse()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);

            var result = template.Attributes["Auto"].GetValue<bool>();
            
            Assert.False(result);
        }
        
        [Test]
        public void SetValue_KnownBooleanToTrue_ReturnsTrue()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);
            template.CheckOut();

            template.Attributes["Auto"].SetValue(true);

            var result = template.Attributes["Auto"].GetValue<bool>();
            Assert.True(result);

            template.ForceClose();
            Assert.False(template.IsCheckedOut());

            result = template.Attributes["Auto"].GetValue<bool>();
            Assert.False(result);
        }
        
        [Test]
        public void ByDataType_KnownTemplateWithBooleans_ReturnsOnlyBooleanAttributes()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);
            
            var attributes = template.Attributes.ByDataType(DataType.Boolean).ToList();
            
            Assert.IsNotEmpty(attributes);
            Assert.True(attributes.All(x => x.DataType == DataType.Boolean));
        }

        [Test]
        public void ByNameContains_TestTemplate_AttributeDataMatchesExpected()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var template = _galaxy.GetObjectByName(tagName);
            
            var attributes = template.Attributes.ByNameContains("Batch").ToList();
            
            Assert.IsNotEmpty(attributes);
            Assert.True(attributes.All(x => x.Name.Contains("Batch")));
        }
    }
}