
using FluentAssertions;
using GCommon.Primitives.Helpers;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.HelperTests
{
    [TestFixture]
    public class HexParserTests
    {
        [Test]
        public void Construction_0x01FF_ShouldHaveExpectedProperties()
        {
            const string hex = "0x01FF";

            var reader = new HexParser(hex);

            reader.IsArray.Should().BeFalse();
            reader.TypeId.Should().Be(1);
            reader.Header.Value.Should().Be("0x01");
            reader.Data.Value.Should().Be("FF");
            reader.ArrayLength.Should().Be(-1);
            reader.ElementSize.Should().Be(-1);
            reader.ArrayHeader.Should().Be(Hex.Empty);
        }
        
        [Test]
        public void Construction_0x03FF00ABCDE_ShouldHaveExpectedProperties()
        {
            const string hex = "0x03FF00ABCDE";

            var reader = new HexParser(hex);

            reader.IsArray.Should().BeFalse();
            reader.TypeId.Should().Be(3);
            reader.Header.Value.Should().Be("0x03");
            reader.Data.Value.Should().Be("FF00ABCDE");
            reader.ArrayLength.Should().Be(-1);
            reader.ElementSize.Should().Be(-1);
            reader.ArrayHeader.Should().Be(Hex.Empty);
        }
    }
}