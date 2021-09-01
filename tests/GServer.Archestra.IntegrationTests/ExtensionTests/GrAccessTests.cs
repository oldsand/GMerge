using ArchestrA.GRAccess;
using GServer.Archestra.Extensions;
using GServer.Archestra.IntegrationTests.Base;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ExtensionTests
{
    [TestFixture]
    public class GrAccessTests
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
        public void UndoCheckout_WhenCalled_Works()
        {
            var target = _galaxy.GetObjectByName(Known.Templates.ReactorSet.TagName);
            
            target.CheckOut();
            Assert.True(target.CheckoutStatus == ECheckoutStatus.checkedOutToMe);
            
            target.UndoCheckOut();
            Assert.True(target.CheckoutStatus == ECheckoutStatus.notCheckedOut);
        }
        
        /*[Test]
        public void SetUnits_TestTemplate_ShouldSetUnitsProperty()
        {
            var template = _galaxy.GetObjectByName("$Test_Template");
            template.CheckOut();
            var attribute = template.GetAttribute("DoubleTest");
            Assert.NotNull(attribute);
            attribute.EngUnits = "gal/m";
            template.Save();
            var attributeUnits = template.GetAttribute("DoubleTest.EngUnits");
            Assert.AreEqual("gal/m", attributeUnits);
            template.CheckIn("Updated Units");
        }
        
        [Test]
        [TestCase("$Test_Template", "TestIntFromGalaxyAccess")]
        public void Add_And_Remove_Attribute(string templateName, string attributeName)
        {
            var template = _galaxy.GetObjectByName(templateName);
            template.CheckOut();

            template.AddUDA(attributeName, DataType.Integer.ToMxType());
            template.Save();

            var attribute = template.GetAttribute(attributeName);
            attribute?.SetValue(25);
            template.Save();

            template.CheckIn($"Adding Test Attribute {attributeName}");

            var result = template.GetAttribute(attributeName);
            Assert.NotNull(result);
            Assert.AreEqual(attributeName, result.Name);
            Assert.AreEqual(DataType.Integer, result.DataType);

            template.CheckOut();
            template.DeleteUDA(attributeName);
            template.Save();
            template.CheckIn($"Deleting Test Attribute {attributeName}");

            result = template.GetAttribute(attributeName);
            Assert.IsNull(result);
        }
        
        [Test]
        [TestCase("$Test_Template")]
        public void GetScriptAttributes_ValidTagName_ReturnsExpected(string tagName)
        {
            var template = _galaxy.GetObject(tagName);

            var attributes = template.Attributes.ToList();

            var scriptAttributes = attributes.Where(a => a.Name.Contains("Configure"));

            Assert.True(scriptAttributes.Any(a => a.Name == "Configure.ExecuteText"));
        }
        
        [Test]
        [TestCase("$ScriptExtension")]
        public void GetTestTemplateSymbol_ScriptAliasAttributes_ReturnsExpected(string tagName)
        {
            var template = _galaxy.GetObjectByName(tagName);
            var aliasNames = template.GetAttribute("ScriptName.Aliases");
            var aliasReferenceNames = template.GetAttribute("ScriptName.AliasReferences");
            Assert.NotNull(aliasNames);
            Assert.NotNull(aliasReferenceNames);
        }

        [Test]
        [TestCase("$Test_Template_Symbol")]
        public void GetTestTemplateSymbol_SymbolAttributes_ReturnsExpected(string tagName)
        {
            var template = _galaxy.GetObject(tagName);

            var attributes = template.Attributes.ToList();

            var symbolAttributes = attributes.Where(a => a.Name.Contains("Symbol"));
            var visualElementDefinition =
                symbolAttributes.SingleOrDefault(x => x.Name == "Symbol._VisualElementDefinition");
            Assert.NotNull(visualElementDefinition);
        }
        
        [Test]
        public void Attributes_vs_ConfigurableAttributes_AreSubset()
        {
            var template = _galaxy.GetObjectByName("$Test_Template");
            var all = template.Attributes;
            var config = template.ConfigurableAttributes;

            CollectionAssert.AreEquivalent(all, config);
            //CollectionAssert.IsSubsetOf(config, all);
        }*/
    }
}