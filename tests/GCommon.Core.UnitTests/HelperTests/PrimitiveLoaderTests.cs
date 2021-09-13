using FluentAssertions;
using GCommon.Core.Enumerations;
using GCommon.Core.Helpers;
using NUnit.Framework;

namespace GCommon.Core.UnitTests.HelperTests
{
    [TestFixture]
    public class PrimitiveLoaderTests
    {
        [Test]
        public void Load_AreaTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.Area);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_GalaxyTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.Galaxy);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_SequencerTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.Sequencer);
            
            attributes.Should().NotBeEmpty();
        }

        [Test]
        public void Load_SymbolTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.Symbol);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AnalogDeviceTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.AnalogDevice);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AppEngineTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.AppEngine);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AutoImportTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.AutoImport);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_ClientControlTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.ClientControl);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DiCommonTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.DiCommon);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DiscreteDeviceTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.DiscreteDevice);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_OpcClientTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.OpcClient);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_SqlDataTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.SqlData);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_UserDefinedTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.UserDefined);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_ViewEngineTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.ViewEngine);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_WinPlatformTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.WinPlatform);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_InTouchProxyTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.InTouchProxy);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_RedundantDiObjectTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.RedundantDiObject);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DdeSuiteLinkClientTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.DdeSuiteLinkClient);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_InTouchViewAppTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.ForTemplate(Template.InTouchViewApp);
            
            attributes.Should().NotBeEmpty();
        }
    }
}