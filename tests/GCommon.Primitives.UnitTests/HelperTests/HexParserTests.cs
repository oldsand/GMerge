
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
        
        [Test]
        [TestCase("0x41000000000400020000000000FFFF0000FFFF")]
        [TestCase("0x42000000000500040000000100000002000000030000000400000005000000")]
        public void ParseDataArray_ValidArrayHex_ShouldHaveExpectedElementCount(string input)
        {
            var hex = new Hex(input);
            var parser = new HexParser(hex);

            var collection = parser.ParseDataArray();

            collection.Should().HaveCount(parser.ArrayLength);
        }

        [Test]
        [TestCase("0x45000000000500040000003B000000053600000032000000530079006D0062006F006C003A00520065006100630074006F0072004200610063006B00670072006F0075006E00640000004700000005420000003E000000530079006D0062006F006C003A006D0065002E00520065006100630074006F0072002E005200650061006300740044006900730070006C006100790000003F000000053A00000036000000530079006D0062006F006C003A006D0065002E00530074006F007200610067006500540061006E006B002E00540061006E006B0000004B000000054600000042000000530079006D0062006F006C003A0050006900700065003100500069007000650047007200650065006E00530068006F00720074005F006E006F0053007400730000003700000005320000002E000000530079006D0062006F006C003A00530077006900740063006800320050006F0073006900740069006F006E000000")]
        public void ParseDataArray(string input)
        {
            var hex = new Hex(input);
            var parser = new HexParser(hex);

            var collection = parser.ParseDataArray();

            collection.Should().NotBeEmpty();
        }
    }
}