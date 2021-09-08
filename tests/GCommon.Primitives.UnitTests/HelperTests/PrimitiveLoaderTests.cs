using FluentAssertions;
using GCommon.Primitives.Enumerations;
using GCommon.Primitives.Helpers;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.HelperTests
{
    [TestFixture]
    public class PrimitiveLoaderTests
    {
        [Test]
        public void Load_AreaTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.Area);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_GalaxyTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.Galaxy);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_SequencerTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.Sequencer);
            
            attributes.Should().NotBeEmpty();
        }

        [Test]
        public void Load_SymbolTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.Symbol);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AnalogDeviceTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.AnalogDevice);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AppEngineTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.AppEngine);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_AutoImportTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.AutoImport);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_ClientControlTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.ClientControl);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DiCommonTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.DiCommon);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DiscreteDeviceTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.DiscreteDevice);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_OpcClientTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.OpcClient);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_SqlDataTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.SqlData);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_UserDefinedTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.UserDefined);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_ViewEngineTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.ViewEngine);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_WinPlatformTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.WinPlatform);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_InTouchProxyTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.InTouchProxy);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_RedundantDiObjectTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.RedundantDiObject);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_DdeSuiteLinkClientTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.DdeSuiteLinkClient);
            
            attributes.Should().NotBeEmpty();
        }
        
        [Test]
        public void Load_InTouchViewAppTemplate_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.InTouchViewApp);
            
            attributes.Should().NotBeEmpty();
        }
    }
}