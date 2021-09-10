using System.Linq;
using ArchestrA.GRAccess;
using FluentAssertions;
using GServer.Archestra.Extensions;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ExtensionTests
{
    [TestFixture]
    public class ObjectExtensionTests
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
        public void IsCheckedOut_WhenCalled_ReturnsExpectedResult()
        {
            var template = _galaxy.GetObjectByName(Known.Templates.ReactorSet.TagName);
            
            template.CheckOut();
            Assert.IsTrue(template.IsCheckedOut());

            template.UndoCheckOut();
            Assert.IsFalse(template.IsCheckedOut());
        }

        [Test]
        public void IsTemplate_KnownTemplate_ReturnsTrue()
        {
            var template = _galaxy.GetObjectByName(Known.Templates.ReactorSet.TagName);

            var result = template.IsTemplate();
            
            Assert.True(result);
        }

        [Test]
        public void IsTemplate_KnownInstance_ReturnsTrue()
        {
            var instance = _galaxy.GetObjectByName(Known.Instances.DrumConveyor);

            var result = instance.IsTemplate();
            
            Assert.False(result);
        }

        [Test]
        public void GetAttribute_KnownTemplateAttribute_ReturnsExpectedInstance()
        {
            var template = _galaxy.GetObjectByName(Known.Templates.ReactorSet.TagName);
            var attributeName = Known.Templates.ReactorSet.Attributes.SingleOrDefault(x => x.Name == "Auto")?.Name;
            
            var attribute = template.GetAttribute(attributeName);
            
            Assert.AreEqual(attributeName, attribute.Name);
        }

        [Test]
        public void ForceClose_WhenCalled_ReturnsOriginal()
        {
            var target = _galaxy.GetObjectByName(Known.Templates.ReactorSet.TagName);
            target.CheckOut();
            
            target.Attributes["BatchNum"].Description = "This is a test";
            target.Attributes["BatchNum"].SetValue(22);
            
            target.ForceClose();
            
            Assert.AreEqual("No Data", target.Attributes["BatchNum"].Description);
            Assert.AreEqual(0, target.Attributes["BatchNum"].GetValue<int>());
        }

        [Test]
        public void GetVisualElementDefinition_KnownSymbol_ShouldNotBeNull()
        {
            var target = _galaxy.GetSymbolByName(Known.Symbols.ProportionalValve).AsObject();

            var definition = target.GetVisualDefinition();

            definition.Should().NotBeNull();
        }

        [Test]
        public void MapGraphic_KnownGraphic_ShouldNotBeNull()
        {
            var target = _galaxy.GetSymbolByName(Known.Symbols.ProportionalValve);

            var graphic = target.MapGraphic();

            graphic.Should().NotBeNull();
        }
    }
}