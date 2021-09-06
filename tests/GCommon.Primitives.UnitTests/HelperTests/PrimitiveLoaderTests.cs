using FluentAssertions;
using GCommon.Primitives.Enumerations;
using GCommon.Primitives.Helpers;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests
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
        public void Load_GalaxySequencer_ShouldNotBeEmpty()
        {
            var attributes = PrimitiveLoader.Load(Template.Sequencer);
            
            attributes.Should().NotBeEmpty();
        }
    }
}