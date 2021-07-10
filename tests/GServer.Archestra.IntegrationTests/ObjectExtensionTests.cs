using System;
using System.Linq;
using GServer.Archestra.Entities;
using GServer.Archestra.Extensions;
using GCommon.Core.Extensions;
using GCommon.Primitives;
using GTest.Core;
using GServer.Archestra;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class RepositoryObjectTests
    {
        private GalaxyRepository _galaxy;

        [SetUp]
        public void Setup()
        {
            _galaxy = new GalaxyRepository(Settings.CurrentTestGalaxy);
            _galaxy.Login(Settings.CurrentTestUser);
        }

        [Test]
        public void Checkout_WhenCalled_CheckoutPropertiesValid()
        {
            var template = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
            
            template.CheckOut();

            Assert.IsTrue(template.IsCheckedOut());
            Assert.AreEqual($"{Environment.UserDomainName}\\{Environment.UserName}", template.checkedOutBy);

            template.UndoCheckOut();

            Assert.IsFalse(template.IsCheckedOut());
            Assert.AreEqual(string.Empty, template.checkedOutBy);
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
        [TestCase("$Test_Template")]
        public void GetPredefinedUdaAndExtensionAttributes(string tagName)
        {
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

        [Test]
        public void GetAttributes_DataQualityType_ReturnsExpected()
        {
            var template = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
            var attributes = template.ConfigurableAttributes.ByDataType(DataType.DataQuality).ToList();
            Assert.IsEmpty(attributes);
        }
        
        [Test]
        public void GetAttributes_StatusType_ReturnsExpected()
        {
            var template = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
            var attributes = template.ConfigurableAttributes.ByDataType(DataType.StatusType).ToList();
            Assert.IsEmpty(attributes);
        }

        [Test]
        public void SetUnits_TestTemplate_ShouldSetUnitsProperty()
        {
            var template = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
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
        [TestCase("Configure")]
        public void GetExtensionAttributes_TestTemplate_AttributeDataMatchesExpected(string attributeName)
        {
            var template = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
            var attributes = template.Attributes.ByNameContains(attributeName);
            Assert.NotNull(attributes);
            foreach (var attribute in attributes)
            {
                Assert.NotNull(attribute.Name);
                Assert.NotNull(attribute.DataType);
                Assert.NotNull(attribute.Category);
                Assert.NotNull(attribute.Security);
                Assert.NotNull(attribute.Locked);
            }
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
            var template = _galaxy.GetGalaxy().GetObjectByName(tagName);
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
        public void SerializeTemplate_WhenCalled_ReturnsCorrectXml()
        {
            var siteData = (GalaxyObject) _galaxy.GetObject("$Site_Data");
            var data = siteData.Serialize();
            Assert.NotNull(data);
        }

        [Test]
        [TestCase("$Test_Template", "TestIntFromGalaxyAccess")]
        public void Add_And_Remove_Attribute(string templateName, string attributeName)
        {
            var template = _galaxy.GetGalaxy().GetObjectByName(templateName);
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
        public void Attributes_vs_ConfigurableAttributes_AreSubset()
        {
            var template = _galaxy.GetGalaxy().GetObjectByName("$Test_Template");
            var all = template.Attributes;
            var config = template.ConfigurableAttributes;

            CollectionAssert.AreEquivalent(all, config);
            //CollectionAssert.IsSubsetOf(config, all);
        }

        [Test]
        public void GetExtensions_DerivedTemplate001_ReturnsExpected()
        {
            var template = (GalaxyObject) _galaxy.GetObject("$DerivedTemplate001");
            var intTestAttributes = template.Attributes.Where(a => a.Name.StartsWith($"IntTest.")).ToList();
            Assert.NotNull(intTestAttributes);
        }

        [Test]
        [TestCase("$DerivedTemplate")]
        [TestCase("DerivedInstance")]
        public void Create_And_Delete_Some_Object(string tagName)
        {
            var created = _galaxy.GetGalaxy().CreateObject(tagName, "$UserDefined");
            
            Assert.NotNull(created);
            Assert.AreEqual(tagName, created.Tagname);
            
            created.Delete();

            var result = _galaxy.GetGalaxy().GetObjectByName(tagName);
            
            Assert.IsNull(result);
        }
    }
}