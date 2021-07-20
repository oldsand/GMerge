using System.Linq;
using AutoFixture;
using GServer.Archestra.Extensions;
using GServer.Archestra.Helpers;
using NUnit.Framework;

namespace GServer.Archestra.UnitTests.ExtensionTests
{
    [TestFixture]
    public class MxValueBooleanTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void GetValue_SingleValue_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<bool>();
            var mxValue = Mx.Create<bool>();

            var result = mxValue.GetValue<bool>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetValue_ArrayValue_ReturnsExpectedValue()
        {
            var count = _fixture.Create<int>();
            var expected = _fixture.CreateMany<bool>(count).ToArray();
            var mxValue = Mx.Create<bool[]>();
            mxValue.SetValue(expected);

            var result = mxValue.GetValue<bool[]>();

            Assert.NotNull(result);
            Assert.AreEqual(expected, result);
            Assert.That(result, Has.Length.EqualTo(count));
        }

        [Test]
        public void GetValue_AsObject_ReturnsExpectedValue()
        {
            var expected = _fixture.Create<bool>();
            var mxValue = Mx.Create<bool>();

            var result = mxValue.GetValue<object>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetValue_ArrayToDelimitedString_ReturnsExpectedValue()
        {
            /*var count = _fixture.Create<int>();
            var expected = _fixture.CreateMany<bool>(count).ToArray();

            for (var i = 1; i <= count; i++)
            {
                var value = new MxValueClass();
                value.PutBoolean(expected[i - 1]);
                _mxValue.PutElement(i, value);
            }

            var result = _mxValue.GetValue<object>();

            var second = ((IEnumerable) result).Cast<object>()
                .Select(x => x.ToString())
                .ToArray();

            var data = string.Join(",", second);

            Assert.NotNull(data);
            Assert.That(data, Contains.Substring("True"));*/
        }

        [Test]
        public void SetValue_SingleValue_ReturnsExpectedValue()
        {
            var mxValue = Mx.Create<bool>();
            
            mxValue.SetValue(true);

            var result = mxValue.GetValue<bool>();
            Assert.True(result);
        }

        [Test]
        public void SetValue_ArrayValue_ReturnsExpectedValue()
        {
            var count = _fixture.Create<int>();
            var expected = _fixture.CreateMany<bool>(count).ToArray();
            var mxValue = Mx.Create<bool[]>();

            mxValue.SetValue(expected);

            var result = mxValue.GetValue<bool[]>();

            Assert.NotNull(result);
            Assert.That(result, Has.Length.EqualTo(count));
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetValue_AsObjectArgument_ReturnsExpectedValue()
        {
            object value = true;
            var mxValue = Mx.Create<bool>();
            
            mxValue.SetValue(value);

            var result = mxValue.GetValue<bool>();
            Assert.True(result);
        }

        [Test]
        public void SetValue_AsStringArgument_ReturnsExpectedValue()
        {
            var mxValue = Mx.Create<bool>();
            
            mxValue.SetValue("true");

            var result = mxValue.GetValue<bool>();
            Assert.True(result);
        }
    }
}