using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GCommon.Primitives.Enumerations;
using GCommon.Primitives.Structs;
using GServer.Archestra.Extensions;
using GServer.Archestra.Helpers;
using NUnit.Framework;

namespace GServer.Archestra.UnitTests
{
    [TestFixture]
    public class MxExtensionTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void GetSet_Boolean_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<bool>();
            var mxValue = MxFactory.Create<bool>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<bool>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_BooleanArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<bool>().ToArray();
            var mxValue = MxFactory.Create<bool[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<bool[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_BooleanAsString_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<bool>();
            var mxValue = MxFactory.Create<bool>();

            mxValue.SetValue(expected.ToString());
            var result = mxValue.GetValue<bool>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_Integer_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<int>();
            var mxValue = MxFactory.Create<int>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<int>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_IntegerArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<int>().ToArray();
            var mxValue = MxFactory.Create<int[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<int[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_IntegerAsString_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<int>();
            var mxValue = MxFactory.Create<int>();

            mxValue.SetValue(expected.ToString());
            var result = mxValue.GetValue<int>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_Float_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<float>();
            var mxValue = MxFactory.Create<float>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<float>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_FloatArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<float>().ToArray();
            var mxValue = MxFactory.Create<float[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<float[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_FloatAsString_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<float>();
            var mxValue = MxFactory.Create<float>();

            mxValue.SetValue(expected.ToString(CultureInfo.CurrentCulture));
            var result = mxValue.GetValue<float>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_Double_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<double>();
            var mxValue = MxFactory.Create<double>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<double>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_DoubleArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<double>().ToArray();
            var mxValue = MxFactory.Create<double[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<double[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_DoubleAsString_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<double>();
            var mxValue = MxFactory.Create<double>();

            mxValue.SetValue(expected.ToString(CultureInfo.CurrentCulture));
            var result = mxValue.GetValue<double>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_String_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<string>();
            var mxValue = MxFactory.Create<string>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<string>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_StringArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<string>().ToArray();
            var mxValue = MxFactory.Create<string[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<string[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_DateTime_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<DateTime>();
            var mxValue = MxFactory.Create<DateTime>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<DateTime>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_DateTimeArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<DateTime>().ToArray();
            var mxValue = MxFactory.Create<DateTime[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<DateTime[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_DateTimeAsString_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<DateTime>();
            var mxValue = MxFactory.Create<DateTime>();

            mxValue.SetValue(expected.ToString(CultureInfo.CurrentCulture));
            var result = mxValue.GetValue<DateTime>();
            
            result.ToString(CultureInfo.CurrentCulture).Should().Be(expected.ToString(CultureInfo.CurrentCulture));
        }

        [Test]
        public void GetSet_TimeSpan_ShouldBeExpectedValue()
        {
            var expected = _fixture.Create<TimeSpan>();
            var mxValue = MxFactory.Create<TimeSpan>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<TimeSpan>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_TimeSpanArray_ShouldBeEqualToExpected()
        {
            var expected = _fixture.CreateMany<TimeSpan>().ToArray();
            var mxValue = MxFactory.Create<TimeSpan[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<TimeSpan[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_TimeSpanAsString_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<TimeSpan>();
            var mxValue = MxFactory.Create<TimeSpan>();

            mxValue.SetValue(expected.ToString());
            var result = mxValue.GetValue<TimeSpan>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_Reference_ShouldBeExpectedValue()
        {
            var expected = new Reference("Some.Reference");
            var mxValue = MxFactory.Create<Reference>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<Reference>();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_ReferenceArray_ShouldBeEqualToExpected()
        {
            var expected = new List<Reference>
            {
                new Reference("Some.Reference1"),
                new Reference("Some.Reference2"),
                new Reference("Some.Reference3")
            };
            var mxValue = MxFactory.Create<Reference[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<Reference[]>();
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_DataType_ShouldBeExpectedValue()
        {
            var expected = DataType.Float;
            var mxValue = MxFactory.Create<DataType>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<DataType>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_DataTypeArray_ShouldBeEqualToExpected()
        {
            var expected = new List<DataType>
            {
                DataType.Boolean,
                DataType.Integer,
                DataType.String
            };
            var mxValue = MxFactory.Create<DataType[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<DataType[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetSet_SecurityClassification_ShouldBeExpectedValue()
        {
            var expected = SecurityClassification.Operate;
            var mxValue = MxFactory.Create<SecurityClassification>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<SecurityClassification>();

            result.Should().Be(expected);
        }

        [Test]
        public void GetSet_SecurityClassificationArray_ShouldBeEqualToExpected()
        {
            var expected = new List<SecurityClassification>
            {
                SecurityClassification.Configure,
                SecurityClassification.FreeAccess,
                SecurityClassification.VerifiedWrite
            };
            var mxValue = MxFactory.Create<SecurityClassification[]>();

            mxValue.SetValue(expected);
            var result = mxValue.GetValue<SecurityClassification[]>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }
    }
}