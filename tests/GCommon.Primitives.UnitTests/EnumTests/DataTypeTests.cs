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
        public void DataType_Integer_ShouldBeExpectedType()
        {
            var dataType = DataType.Integer;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Integer);
        }
        
        [Test]
        public void DataType_Double_ShouldBeExpectedType()
        {
            var dataType = DataType.Double;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Double);
        }
        
        [Test]
        public void DataType_Float_ShouldBeExpectedType()
        {
            var dataType = DataType.Float;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Float);
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
            var dataType = DataType.Reference;

            dataType.Should().NotBeNull();
            dataType.Should().Be(DataType.Reference);
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