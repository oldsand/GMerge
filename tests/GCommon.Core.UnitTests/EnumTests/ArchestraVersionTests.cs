using FluentAssertions;
using GCommon.Core.Enumerations;
using NUnit.Framework;

namespace GCommon.Core.UnitTests
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