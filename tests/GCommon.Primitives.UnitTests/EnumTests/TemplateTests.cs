using FluentAssertions;
using GCommon.Primitives.Enumerations;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.EnumTests
{
    [TestFixture]
    public class TemplateTests
    {
        [Test]
        public void GetPrimitiveAttributes_WhenCalled_ShouldHaveNotBeEmpty()
        {
            var template = Template.UserDefined;

            var attributes = template.GetAttributes();

            attributes.Should().NotBeEmpty();
        }
    }
}