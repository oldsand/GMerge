using System;
using FluentAssertions;
using GCommon.Primitives.Enumerations;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.EnumTests
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
            var actual = Convert.ToBoolean(DataType.Boolean.ParseHex(input));
            actual.Should().BeTrue();
        }

        [Test]
        [TestCase("0x0100")]
        public void ParseHex_BooleanSingleValue_ShouldBeFalse(string input)
        {
            var actual = Convert.ToBoolean(DataType.Boolean.ParseHex(input));

            actual.Should().BeFalse();
        }
        
        [Test]
        [TestCase("0x41000000000500020000000000FFFF0000FFFFFFFF", new[] {false, true, false, true, true})]
        public void ParseHex_BooleanArray_ShouldBeEquivalentToExpected(string input, bool[] expected)
        {
            var actual = DataType.Boolean.ParseHex(input);
            
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
            var actual = DataType.Integer.ParseHex(input);
            
            actual.Should().Be(expected);
        }
        
         [Test]
        [TestCase("0x42000000000500040000000C0000003200000000000000F6FFFFFF6CD60000", new[] {12, 50, 0, -10, 54892})]
        public void ParseHex_IntegerArrayValue_ShouldBeEquivalentToExpected(string input, int[] expected)
        {
            var actual = DataType.Integer.ParseHex(input);

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
            var actual = DataType.Float.ParseHex(input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase("0x4300000000050004000000D26F9F3FE17A42C2000028411F05C84200000000", new[] {1.2456f, -48.62f, 10.5f, 100.01f, 0.0f})]
        public void ParseHex_FloatArrayValue_ShouldBeEquivalentToExpected(string input, float[] expected)
        {
            var actual = DataType.Float.ParseHex(input);
            
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
            var actual = DataType.Double.ParseHex(input);
            
            actual.Should().Be(expected);
        }
        
        [Test]
        [TestCase("0x4400000000050008000000F241CF66D5E7EE3F1D5A643BDF8752C062105839B40059400000000000000000333333333333F33F",
            new[] {0.9658, -74.123, 100.011, 0.0, 1.2})]
        public void ParseHex_DoubleArrayValue_ShouldBeEquivalentToExpected(string input, double[] expected)
        {
            var actual = DataType.Double.ParseHex(input);
            
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
        public void DataType_Time_ShouldBeExpectedType()
        {
            var dataType = DataType.Time;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Time);
        }
        
        [Test]
        public void DataType_ElapsedTime_ShouldBeExpectedType()
        {
            var dataType = DataType.ElapsedTime;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.ElapsedTime);
        }
        
        [Test]
        public void DataType_Reference_ShouldBeExpectedType()
        {
            var dataType = DataType.ReferenceType;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.ReferenceType);
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