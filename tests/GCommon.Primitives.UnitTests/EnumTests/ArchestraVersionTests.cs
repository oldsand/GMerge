using FluentAssertions;
using GCommon.Primitives.Enumerations;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests
{
    [TestFixture]
    public class ArchestraVersionTests
    {
        [Test]
        public void List_ShouldNotBeEmpty()
        {
            var version = ArchestraVersion.List;

            version.Should().NotBeEmpty();
        }
    }
}