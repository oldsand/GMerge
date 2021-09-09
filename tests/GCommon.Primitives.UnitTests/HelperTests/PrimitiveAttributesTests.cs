using FluentAssertions;
using GCommon.Primitives.Enumerations;
using GCommon.Primitives.Helpers;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.HelperTests
{
    [TestFixture]
    public class PrimitiveAttributesTests
    {
        [Test]
        public void Load_AreaTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.Area);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_GalaxyTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.Galaxy);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_SequencerTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.Sequencer);
            
            attributes.Should().NotBeEmpty();
        }

        [Test]
        public void Load_SymbolTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.Symbol);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AnalogDeviceTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.AnalogDevice);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AppEngineTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.AppEngine);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AutoImportTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.AutoImport);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_ClientControlTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.ClientControl);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DiCommonTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.DiCommon);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DiscreteDeviceTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.DiscreteDevice);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_OpcClientTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.OpcClient);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_SqlDataTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.SqlData);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_UserDefinedTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.UserDefined);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_ViewEngineTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.ViewEngine);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_WinPlatformTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.WinPlatform);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_InTouchProxyTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.InTouchProxy);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_RedundantDiObjectTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.RedundantDiObject);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DdeSuiteLinkClientTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.DdeSuiteLinkClient);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_InTouchViewAppTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveAttributes.ForTemplate(Template.InTouchViewApp);
            
            attributes.Should().NotBeEmpty();
        }
    }
}