using System;
using System.Collections;
using System.Linq;
using ArchestrA.GRAccess;
using GServer.Archestra.Extensions;
using GCommon.Core.Extensions;
using GCommon.Primitives;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class MxValueExtensionsUnitTest
    {
        [Test]
        public void GetValueBoolean_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutBoolean(false);

            var result = mxValue.GetValue<bool>();

            Assert.False(result);
        }

        [Test]
        public void GetValueBoolean_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutBoolean(true);
                array.PutElement(i, value);
            }

            var result = array.GetValue<bool[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueBoolean_AsObject_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutBoolean(false);

            var result = mxValue.GetValue<object>();

            Assert.False((bool) result);
        }
        
        [Test]
        public void GetValueBoolean_ArrayToDelimitedString_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutBoolean(true);
                array.PutElement(i, value);
            }

            var result = array.GetValue<object>();
            
            var second = ((IEnumerable)result).Cast<object>()
                .Select(x => x.ToString())
                .ToArray();

            var data = string.Join(",", second);

            Assert.NotNull(data);
            Assert.That(data, Contains.Substring("True"));
        }

        [Test]
        public void GetValueInteger_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutInteger(123);

            var result = mxValue.GetValue<int>();

            Assert.AreEqual(123, result);
        }

        [Test]
        public void GetValueInteger_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutInteger(i * 2);
                array.PutElement(i, value);
            }

            var result = array.GetValue<int[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueFloat_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutFloat(1.123f);

            var result = mxValue.GetValue<float>();

            Assert.AreEqual(1.123f, result);
        }

        [Test]
        public void GetValueFloat_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutFloat(i * 1.5F);
                array.PutElement(i, value);
            }

            var result = array.GetValue<float[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueDouble_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutDouble(1.123456);

            var result = mxValue.GetValue<double>();

            Assert.AreEqual(1.123456, result);
        }

        [Test]
        public void GetValueDouble_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutDouble(i * 1.123);
                array.PutElement(i, value);
            }

            var result = array.GetValue<double[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueString_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutString("This is a test");

            var result = mxValue.GetValue<string>();

            Assert.AreEqual("This is a test", result);
        }

        [Test]
        public void GetValueString_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutString($"Test #{i}");
                array.PutElement(i, value);
            }

            var result = array.GetValue<string[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueString_ArrayToDelimited_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutString($"Test #{i}");
                array.PutElement(i, value);
            }

            var result = array.GetValue<object>();

            var data = string.Join(",", result.ConvertTo<string[]>());

            Assert.NotNull(data);
        }

        [Test]
        public void GetValueDateTime_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            var now = DateTime.Now;
            mxValue.PutTime(now.ToVbFileTime());

            var result = mxValue.GetValue<DateTime>();

            Assert.AreEqual(now, result);
        }

        [Test]
        public void GetValueDateTime_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutTime(DateTime.Now.ToVbFileTime());
                array.PutElement(i, value);
            }

            var result = array.GetValue<DateTime[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueTimeSpan_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            var expected = TimeSpan.FromDays(4);
            mxValue.PutElapsedTime(expected.ToVbLargeInteger());

            var result = mxValue.GetValue<TimeSpan>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetValueTimeSpan_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutElapsedTime(TimeSpan.FromDays(4).ToVbLargeInteger());
                array.PutElement(i, value);
            }

            var result = array.GetValue<TimeSpan[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueDataType_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            var expected = DataType.String;
            mxValue.PutMxDataType(expected.ToMxType());

            var result = mxValue.GetValue<DataType>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetValueDataType_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutMxDataType(DataType.String.ToMxType());
                array.PutElement(i, value);
            }

            var result = array.GetValue<DataType[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueSecurityClassification_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            var expected = SecurityClassification.Tune;
            mxValue.PutMxSecurityClassification(expected.ToMxType());

            var result = mxValue.GetValue<SecurityClassification>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetValueSecurityClassification_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutMxSecurityClassification(SecurityClassification.Tune.ToMxType());
                array.PutElement(i, value);
            }

            var result = array.GetValue<SecurityClassification[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValueCustomEnum_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            var expected = "SomeEnumName";
            mxValue.PutCustomEnum(expected, 0, 0, 0);

            var result = mxValue.GetValue<string>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetValueCustomEnum_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutCustomEnum("SomeEnumName", 0, 0, 0);
                array.PutElement(i, value);
            }

            var result = array.GetValue<string[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(20));
        }

        [Test]
        public void GetValue_SingleValueArrayArgument_ThrowArgumentException()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutBoolean(true);
                array.PutElement(i, value);
            }

            Assert.Throws<InvalidCastException>(() => { array.GetValue<bool>(); });
        }

        [Test]
        public void GetValue_ArrayValueSingleArgument_ThrowArgumentException()
        {
            var mxValue = new MxValueClass();
            mxValue.PutBoolean(false);

            Assert.Throws<InvalidCastException>(() => { mxValue.GetValue<bool[]>(); });
        }

        [Test]
        public void SetValueBoolean_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutBoolean(false);

            mxValue.SetValue(true);

            var result = mxValue.GetValue<bool>();
            Assert.True(result);
        }

        [Test]
        public void SetValueBoolean_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutBoolean(true);
                array.PutElement(i, value);
            }

            var data = new[] {true, false, true, false, true};

            array.SetValue(data);

            var result = array.GetValue<bool[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueBoolean_AsObjectArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutBoolean(false);

            object value = true;
            mxValue.SetValue(value);

            var result = mxValue.GetValue<bool>();
            Assert.True(result);
        }

        [Test]
        public void SetValueBoolean_AsStringArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutBoolean(false);

            mxValue.SetValue("true");

            var result = mxValue.GetValue<bool>();
            Assert.True(result);
        }

        [Test]
        public void SetValueInteger_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutInteger(123);

            mxValue.SetValue(321);

            var result = mxValue.GetValue<int>();
            Assert.AreEqual(321, result);
        }

        [Test]
        public void SetValueInteger_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutInteger(i);
                array.PutElement(i, value);
            }

            var data = new[] {5, 4, 3, 2, 1};

            array.SetValue(data);

            var result = array.GetValue<int[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueFloat_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutFloat(123.5f);

            mxValue.SetValue(1.12512f);

            var result = mxValue.GetValue<float>();
            Assert.AreEqual(1.12512f, result);
        }

        [Test]
        public void SetValueFloat_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutFloat(i * 1.236f);
                array.PutElement(i, value);
            }

            var data = new[] {1.1f, 2.2f, 3.3f, 4.4f, 5.5f};

            array.SetValue(data);

            var result = array.GetValue<float[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueDouble_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutDouble(5.46189);

            mxValue.SetValue(6.251984);

            var result = mxValue.GetValue<double>();
            Assert.AreEqual(6.251984, result);
        }

        [Test]
        public void SetValueDouble_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutDouble(i * 1.23);
                array.PutElement(i, value);
            }

            var data = new[] {5.5, 4.4, 3.3, 2.2, 1.1};

            array.SetValue(data);

            var result = array.GetValue<double[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueString_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutString("This is a test");

            mxValue.SetValue("This is a different test");

            var result = mxValue.GetValue<string>();
            Assert.AreEqual("This is a different test", result);
        }

        [Test]
        public void SetValueString_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutString($"Test #{i}");
                array.PutElement(i, value);
            }

            var data = new[] {"Test #5", "Test #4", "Test #3", "Test #2", "Test #1"};

            array.SetValue(data);

            var result = array.GetValue<string[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueString_DelimitedString_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutString($"Test #{i}");
                array.PutElement(i, value);
            }

            var data = ("Test #5, Test #4, Test #3, Test #2, Test #1").Split(',');

            array.SetValue(data);

            var result = array.GetValue<string[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueDateTime_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutTime(DateTime.Today.ToVbFileTime());

            var expected = DateTime.Now;
            mxValue.SetValue(expected);

            var result = mxValue.GetValue<DateTime>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetValueDateTime_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutTime(DateTime.Today.ToVbFileTime());
                array.PutElement(i, value);
            }

            var data = new[]
            {
                DateTime.Now.AddMinutes(1),
                DateTime.Now.AddMinutes(2),
                DateTime.Now.AddMinutes(3),
                DateTime.Now.AddMinutes(4),
                DateTime.Now.AddMinutes(5)
            };

            array.SetValue(data);

            var result = array.GetValue<DateTime[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }

        [Test]
        public void SetValueTimeSpan_SingleArgument_ReturnsExpectedValue()
        {
            var mxValue = new MxValueClass();
            mxValue.PutElapsedTime(TimeSpan.FromDays(4).ToVbLargeInteger());

            var expected = TimeSpan.FromHours(15);
            mxValue.SetValue(expected);

            var result = mxValue.GetValue<TimeSpan>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetValueTimeSpan_ArrayArgument_ReturnsExpectedValue()
        {
            var array = new MxValueClass();
            for (var i = 1; i <= 20; i++)
            {
                var value = new MxValueClass();
                value.PutElapsedTime(TimeSpan.FromDays(i).ToVbLargeInteger());
                array.PutElement(i, value);
            }

            var data = new[]
            {
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(2),
                TimeSpan.FromHours(3),
                TimeSpan.FromHours(4),
                TimeSpan.FromHours(5)
            };

            array.SetValue(data);

            var result = array.GetValue<TimeSpan[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(5));
            Assert.AreEqual(data, result);
        }
    }
}