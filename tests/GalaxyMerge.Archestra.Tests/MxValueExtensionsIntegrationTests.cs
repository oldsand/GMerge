using ArchestrA.GRAccess;
using GServer.Archestra.Extensions;
using GalaxyMerge.Test.Core;
using GServer.Archestra;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class MxValueExtensionsIntegrationTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(Settings.CurrentTestGalaxy);
            _galaxy.Login(Settings.CurrentTestUser);
        }
        
        [Test]
        public void SetValueReference_SingleArgument_ReturnsExpectedValue()
        {
            var knownObject = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
            var mxReference = knownObject.GetAttribute("BoolTest.InputSource");
            
            /*mxReference.SetValue("MyNewReferenceString");

            var result = mxReference.GetValue<string>();
            Assert.True(result);*/
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
            var knownObject = _galaxy.GetGalaxy().GetObjectByName("$Site_Data");
            var testStruct = knownObject.GetAttribute("DeployedNode._refAttrKey");
            var value = testStruct?.value;
            Assert.NotNull(value);
        }
    }
}