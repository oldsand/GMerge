using System;
using AutoFixture;
using FluentAssertions;
using GCommon.Differencing.UnitTests.TestClasses;
using NUnit.Framework;

namespace GCommon.Differencing.UnitTests
{
    [TestFixture]
    public class DifferenceTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void Create_PropertyExpressionSimpleString_ResultShouldHaveExpectedData()
        {
            var first = _fixture.Create<string>();
            var second = _fixture.Create<string>();

            var difference = Difference<string>.Create(first, second, x => x);

            difference.Left.Should().Be(first);
            difference.Right.Should().Be(second);
            difference.PropertyType.Should().Be(typeof(string));
            difference.ObjectType.Should().Be(typeof(string));
            difference.PropertyName.Should().Be(string.Empty);
        }
        
        [Test]
        public void Create_PropertyExpressionSimpleDateTime_ResultShouldHaveExpectedData()
        {
            var first = _fixture.Create<DateTime>();
            var second = _fixture.Create<DateTime>();

            var difference = Difference<DateTime>.Create(first, second, x => x);

            difference.Left.Should().Be(first);
            difference.Right.Should().Be(second);
            difference.PropertyType.Should().Be(typeof(DateTime));
            difference.ObjectType.Should().Be(typeof(DateTime));
            difference.PropertyName.Should().Be(string.Empty);
        }
        
        [Test]
        public void Create_PropertyExpressionComplexType_ResultShouldHaveExpectedData()
        {
            var first = _fixture.Create<Car>();
            var second = _fixture.Create<Car>();

            var difference = Difference<string>.Create(first, second, x => x.Model);

            difference.Left.Should().Be(first.Model);
            difference.Right.Should().Be(second.Model);
            difference.PropertyType.Should().Be(typeof(string));
            difference.ObjectType.Should().Be(typeof(Car));
            difference.PropertyName.Should().Be("Model");
        }

        [Test]
        public void Between_SimpleIntegerTypeDiffers_ResultShouldHaveSingleCount()
        {
            var first = _fixture.Create<int>();
            var second = _fixture.Create<int>();

            var result = Difference<int>.Between(first, second);

            result.Should().HaveCount(1);
        }
        
        [Test]
        public void Between_SimpleStringTypeDiffers_ResultShouldHaveSingleCount()
        {
            var first = _fixture.Create<string>();
            var second = _fixture.Create<string>();

            var result = Difference<string>.Between(first, second);

            result.Should().HaveCount(1);
        }
        
    }
}