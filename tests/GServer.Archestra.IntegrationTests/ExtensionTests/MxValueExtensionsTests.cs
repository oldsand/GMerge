using ArchestrA.GRAccess;
using GServer.Archestra.Extensions;
using GServer.Archestra.IntegrationTests.Base;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ExtensionTests
{
    [TestFixture]
    public class MxValueExtensionsTests
    {
        private static IGalaxy _galaxy;
        private GRAccessAppClass _grAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _grAccess = new GRAccessAppClass();
            _galaxy = _grAccess.QueryGalaxies()[TestConfig.GalaxyName];
            _galaxy.Login(string.Empty, string.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
            _galaxy = null;
        }
        
        [Test]
        public void SetValueReference_SingleArgument_ReturnsExpectedValue()
        {
            var tagName = Known.Templates.ReactorSet.TagName;
            var knownObject = _galaxy.GetObjectByName(tagName);
            
            var reference = knownObject.GetAttribute("Auto.InputSource");
            
            reference.SetValue("InputReference");

            var result = reference.GetValue<string>();
            Assert.AreEqual("InputReference", result);
        }

        [Test]
        public void SetValueReference_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutBoolean(true);
                array.PutElement(i, value);
            }
            
            var data = new[] {true, false, true, false, true};

            array.SetValue(data);
            
            var result = array.GetValue<bool[]>();
            
            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
        }
        
        [Test]
        public void SetValue_CustomStruct_ReturnExpected()
        {
            var knownObject = _galaxy.GetObjectByName("$Site_Data");
            var testStruct = knownObject.GetAttribute("DeployedNode._refAttrKey");
            var value = testStruct?.value;
            Assert.NotNull(value);
        }
    }
}