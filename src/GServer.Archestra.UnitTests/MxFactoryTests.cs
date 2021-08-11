using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using AutoFixture;
using FluentAssertions;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using GServer.Archestra.Extensions;
using GServer.Archestra.Internal;
using NUnit.Framework;

namespace GServer.Archestra.UnitTests
{
    [TestFixture]
    public class MxFactoryTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
        
        [Test]
        public void CreateBoolean_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<bool>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxBoolean);
        }
        
        [Test]
        public void CreateBoolean_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<bool>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxBoolean);

            var value = mxValue.GetValue<bool>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateBooleanArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<bool[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxBoolean);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateBooleanArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<bool>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxBoolean);
            mxValue.IsArray().Should().BeTrue();
            
            var values = mxValue.GetValue<bool[]>();
            values.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateInteger_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<int>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxInteger);
        }
        
        [Test]
        public void CreateInteger_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<int>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxInteger);

            var value = mxValue.GetValue<int>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateIntegerArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<int[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxInteger);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateIntegerArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<int>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxInteger);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<int[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateDouble_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<double>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxDouble);
        }
        
        [Test]
        public void CreateDouble_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<double>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxDouble);

            var value = mxValue.GetValue<double>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateDoubleArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<double[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxDouble);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateDoubleArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<double>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxDouble);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<double[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateFloat_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<float>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxFloat);
        }
        
        [Test]
        public void CreateFloat_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<float>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxFloat);

            var value = mxValue.GetValue<float>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateFloatArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<float[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxFloat);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateFloatArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<float>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxFloat);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<float[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateString_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<string>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxString);
        }
        
        [Test]
        public void CreateString_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<string>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxString);

            var value = mxValue.GetValue<string>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateStringArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<string[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxString);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateStringArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<string>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxString);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<string[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateDateTime_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<DateTime>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxTime);
        }
        
        [Test]
        public void CreateDateTime_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<DateTime>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxTime);

            var value = mxValue.GetValue<DateTime>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateDateTimeArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<DateTime[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxTime);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateDateTimeArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<DateTime>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxTime);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<DateTime[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateElapsedTime_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<TimeSpan>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxElapsedTime);
        }
        
        [Test]
        public void CreateElapsedTime_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.Create<TimeSpan>();
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxElapsedTime);

            var value = mxValue.GetValue<TimeSpan>();
            value.Should().Be(expected);
        }
        
        [Test]
        public void CreateElapsedTimeArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<TimeSpan[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxElapsedTime);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateElapsedTimeArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = _fixture.CreateMany<TimeSpan>().ToArray();
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxElapsedTime);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<TimeSpan[]>();
            value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateReference_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<Reference>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxReferenceType);
        }

        [Test]
        public void CreateReference_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = new Reference
            {
                FullReference = "Me.TestReference",
                ObjectReference = "Me",
                AttributeReference = "TestReference"
            };
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxReferenceType);

            var value = mxValue.GetValue<Reference>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateReferenceArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<Reference[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxReferenceType);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateReferenceArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = new List<Reference>
            {
                new Reference {FullReference = "Me.Reference1"},
                new Reference {FullReference = "Me.Reference2"},
                new Reference {FullReference = "Me.Reference3"}
            };
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxReferenceType);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<Reference[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateDataType_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<DataType>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxDataTypeEnum);
        }

        [Test]
        public void CreateDataType_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = DataType.String;
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxDataTypeEnum);

            var value = mxValue.GetValue<DataType>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateDataTypeArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<DataType[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxDataTypeEnum);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateDataTypeArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = new List<DataType>
            {
                DataType.NoData,
                DataType.Boolean,
                DataType.Integer
            };
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxDataTypeEnum);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<DataType[]>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateSecurityClassification_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<SecurityClassification>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxSecurityClassificationEnum);
        }

        [Test]
        public void CreateSecurityClassification_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = SecurityClassification.Tune;
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxSecurityClassificationEnum);

            var value = mxValue.GetValue<SecurityClassification>();
            value.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void CreateSecurityClassificationArray_SpecifiedType_ReturnsExpectedDataType()
        {
            var mxValue = MxFactory.Create<SecurityClassification[]>();

            var type = mxValue.GetDataType();
            
            type.Should().Be(MxDataType.MxSecurityClassificationEnum);
            mxValue.IsArray().Should().BeTrue();
        }
        
        [Test]
        public void CreateSecurityClassificationArray_ProvideValue_ReturnsExpectedDataType()
        {
            var expected = new List<SecurityClassification>
            {
                SecurityClassification.Configure,
                SecurityClassification.Undefined,
                SecurityClassification.FreeAccess
            };
            
            var mxValue = MxFactory.Create(expected);

            var type = mxValue.GetDataType();
            type.Should().Be(MxDataType.MxSecurityClassificationEnum);
            mxValue.IsArray().Should().BeTrue();
            
            var value = mxValue.GetValue<SecurityClassification[]>();
            value.Should().BeEquivalentTo(expected);
        }
    }
}