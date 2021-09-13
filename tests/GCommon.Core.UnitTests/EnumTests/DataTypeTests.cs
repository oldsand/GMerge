using System;
using FluentAssertions;
using GCommon.Core.Enumerations;
using GCommon.Core.Helpers;
using GCommon.Core.Structs;
using NUnit.Framework;

namespace GCommon.Core.UnitTests.EnumTests
{
    [TestFixture]
    public class DataTypeTests
    {
        [Test]
        public void DataType_Unknown_ShouldBeExpectedType()
        {
            var dataType = DataType.Unknown;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Unknown);
        }

        [Test]
        public void DataType_Boolean_ShouldBeExpectedType()
        {
            var dataType = DataType.Boolean;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Boolean);
        }

        [Test]
        [TestCase("0x01FF")]
        public void ParseHex_BooleanSingleValue_ShouldBeTrue(string input)
        {
            var actual = Convert.ToBoolean(DataType.Boolean.Parse((Hex)input));
            actual.Should().BeTrue();
        }

        [Test]
        [TestCase("0x0100")]
        public void ParseHex_BooleanSingleValue_ShouldBeFalse(string input)
        {
            var actual = Convert.ToBoolean(DataType.Boolean.Parse((Hex)input));

            actual.Should().BeFalse();
        }
        
        [Test]
        [TestCase("0x41000000000500020000000000FFFF0000FFFFFFFF", new[] {false, true, false, true, true})]
        public void ParseHex_BooleanArray_ShouldBeEquivalentToExpected(string input, bool[] expected)
        {
            var hex = new Hex(input);
            
            var actual = DataType.Boolean.Parse(hex);
            
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void DataType_Integer_ShouldBeExpectedType()
        {
            var dataType = DataType.Integer;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Integer);
        }
        
        
        [Test]
        [TestCase("0x02491D0D00", 859465)]
        [TestCase("0x0201000000", 1)]
        [TestCase("0x0202000000", 2)]
        [TestCase("0x0296000000", 150)]
        [TestCase("0x02B168DE3A", 987654321)]
        [TestCase("0x0200000000", 0)]
        [TestCase("0x02FFFFFFFF", -1)]
        public void ParseHex_IntegerSingleValue_ShouldBeEquivalentToExpected(string input, int expected)
        {
            var actual = DataType.Integer.Parse((Hex)input);
            
            actual.Should().Be(expected);
        }
        
         [Test]
        [TestCase("0x42000000000500040000000C0000003200000000000000F6FFFFFF6CD60000", new[] {12, 50, 0, -10, 54892})]
        public void ParseHex_IntegerArrayValue_ShouldBeEquivalentToExpected(string input, int[] expected)
        {
            var actual = DataType.Integer.Parse((Hex)input);

            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void DataType_Float_ShouldBeExpectedType()
        {
            var dataType = DataType.Float;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Float);
        }

        [Test]
        [TestCase("0x030000C03F", 1.5f)]
        [TestCase("0x0379E9F642", 123.456f)]
        [TestCase("0x030080C8C2", -100.25f)]
        [TestCase("0x0300000000", 0.0f)]
        [TestCase("0x030800803F", 1.000001f)]
        public void ParseHex_FloatSingleValue_ReturnsExpectedValue(string input, float expected)
        {
            var actual = DataType.Float.Parse((Hex)input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase("0x4300000000050004000000D26F9F3FE17A42C2000028411F05C84200000000", new[] {1.2456f, -48.62f, 10.5f, 100.01f, 0.0f})]
        public void ParseHex_FloatArrayValue_ShouldBeEquivalentToExpected(string input, float[] expected)
        {
            var actual = DataType.Float.Parse((Hex)input);
            
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void DataType_Double_ShouldBeExpectedType()
        {
            var dataType = DataType.Double;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Double);
        }

        [Test]
        [TestCase("0x04767E9B0F1940F03F", 1.0156489)]
        [TestCase("0x045BD3BCE394A4CE40", 15689.1632)]
        [TestCase("0x0477BE9F1A6F42B6C0", -5698.434)]
        [TestCase("0x040000000000000000", 0.0)]
        public void ParseHex_DoubleSingleValue_ReturnsExpectedValue(string input, double expected)
        {
            var actual = DataType.Double.Parse((Hex)input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase("0x4400000000050008000000F241CF66D5E7EE3F1D5A643BDF8752C062105839B40059400000000000000000333333333333F33F",
            new[] {0.9658, -74.123, 100.011, 0.0, 1.2})]
        public void ParseHex_DoubleArrayValue_ShouldBeEquivalentToExpected(string input, double[] expected)
        {
            var actual = DataType.Double.Parse((Hex)input);
            
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void DataType_String_ShouldBeExpectedType()
        {
            var dataType = DataType.String;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.String);
        }
        
        [Test]
        [TestCase("0x05220000001E000000540068006900730020004900730020004100200054006500730074000000", "This Is A Test")]
        [TestCase("0x052400000020000000240054006500730074005F00540065006D0070006C0061007400650021000000",
            "$Test_Template!")]
        [TestCase(
            "0x0554000000500000005400680069007300200069007300200061006E006F00740068006500720020002E002E002E002E00200020002000740065007300740020007B0043006F006E006600690067007500720065007D000000",
            "This is another ....   test {Configure}")]
        public void StringParse_SingleValue_ReturnsExpectedValue(string input, string expected)
        {
            var actual = DataType.String.Parse((Hex)input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase(
            "0x450000000005000400000013000000050E0000000A000000540068006900730000000F000000050A000000060000004900730000000D0000000508000000040000004100000013000000050E0000000A000000540065007300740000001500000005100000000C000000410072007200610079000000",
            new[] {"This", "Is", "A", "Test", "Array"})]
        public void StringParse_ArrayValue_ReturnsExpectedValue(string input, string[] expected)
        {
            var actual = DataType.String.Parse((Hex)input);
            
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void DataType_Time_ShouldBeExpectedType()
        {
            var dataType = DataType.Time;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Time);
        }
        
        [Test]
        [TestCase("0x060A0000008076898BA8A0D6010000", "10/12/2020 10:01:21 AM")]
        [TestCase("0x060A00000080C4E32092B0D6010000", "11/1/2020 3:01:11.752 PM")]
        [TestCase("0x060A00000000F05094AF02B3010000", "1/5/1989 12:00:00 AM")]
        public void TimeParse_SingleValue_ReturnsExpectedValue(string input, DateTime expected)
        {
            var actual = DataType.Time.Parse((Hex)input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase("0x460000000005000C0000008072366BBB2DB30100000000601CB2BC44DED6010000000080421B6CD396D6010000000080F2FEE905CCD5010000000080329B268154500100000000")]
        public void TimeParse_ArrayValue_ReturnsExpectedValue(string input)
        {
            var expected = new[]
            {
                new DateTime(1989, 2, 28, 18, 43, 05),
                new DateTime(2020, 12, 29, 18, 43, 05, 894),
                new DateTime(2020, 9, 29, 21, 43, 05),
                new DateTime(2020, 1, 15, 18, 43, 05),
                new DateTime(1900, 12, 29, 18, 43, 05)
            };
            
            var actual = DataType.Time.Parse((Hex)input);
            
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void DataType_ElapsedTime_ShouldBeExpectedType()
        {
            var dataType = DataType.ElapsedTime;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.ElapsedTime);
        }
        
        [Test]
        [TestCase("0x078044208608000000", "01:01:01.0000000")]
        [TestCase("0x073051F35BC5000000", "23:32:45.1230000")]
        public void ElapsedTimeParse_SingleValue_ReturnsExpectedValue(string input, TimeSpan expected)
        {
            var actual = DataType.ElapsedTime.Parse((Hex)input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase("0x470000000005000800000020CB218608000000800D5F0C1100000080CD60921900000000128118220000008056A19E2A000000")]
        public void ElapsedTimeParse_ArrayValue_ReturnsExpectedValue(string input)
        {
            var expected = new[]
            {
                new TimeSpan(00,01, 01, 01, 10),
                new TimeSpan(00,02, 02, 02, 200),
                new TimeSpan(00,03, 03, 03),
                new TimeSpan(00,04, 04, 04),
                new TimeSpan(00,05, 05, 05)
            };
            
            var actual = DataType.ElapsedTime.Parse((Hex)input);
            
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void DataType_Reference_ShouldBeExpectedType()
        {
            var dataType = DataType.ReferenceType;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.ReferenceType);
        }
        
        [Test]
        [TestCase("0x0858000000160000814D0065002E0049006E0074005400650073007400000000000000000000001E000000240054006500730074005F00540065006D0070006C0061007400650000000000000000000000000000000000000000000001", "Me.IntTest")]
        public void ReferenceParse_SingleValue_ReturnsExpectedValue(string input, string expected)
        {
            var actual = (Reference) DataType.ReferenceType.Parse((Hex)input);
            
            actual.FullName.Should().Be(expected);
        }
        
        [Test]
        public void DataType_Status_ShouldBeExpectedType()
        {
            var dataType = DataType.Status;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Status);
        }
        
        [Test]
        public void DataType_DataTypeEnum_ShouldBeExpectedType()
        {
            var dataType = DataType.DataTypeEnum;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.DataTypeEnum);
        }
        
        [Test]
        public void DataType_SecurityClassificationEnum_ShouldBeExpectedType()
        {
            var dataType = DataType.SecurityClassificationEnum;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.SecurityClassificationEnum);
        }
        
        [Test]
        public void DataType_DataQuality_ShouldBeExpectedType()
        {
            var dataType = DataType.DataQuality;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.DataQuality);
        }
        
        [Test]
        public void DataType_QualifiedEnum_ShouldBeExpectedType()
        {
            var dataType = DataType.QualifiedEnum;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.QualifiedEnum);
        }
        
        [Test]
        public void DataType_QualifiedStruct_ShouldBeExpectedType()
        {
            var dataType = DataType.QualifiedStruct;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.QualifiedStruct);
        }
        
        [Test]
        public void DataType_InternationalizedString_ShouldBeExpectedType()
        {
            var dataType = DataType.InternationalizedString;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.InternationalizedString);
        }
        
        [Test]
        public void DataType_BigString_ShouldBeExpectedType()
        {
            var dataType = DataType.BigString;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.BigString);
        }

        [Test]
        public void DataType_List_ShouldNotBeEmpty()
        {
            var list = DataType.List;

            list.Should().NotBeEmpty();
        }
    }
}