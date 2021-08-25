using System;
using System.Linq;
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

            var difference = new Difference<string>(first, right: second);

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

            var difference = new Difference<DateTime>(first, second);

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

            var difference = new Difference<string>(first.Model, second.Model, nameof(first.Model), typeof(Car));

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

        [Test]
        public void Between_ComplexTypeDiffers_ResultShouldHaveSingleCount()
        {
            var first = _fixture.Create<Car>();
            var second = _fixture.Create<Car>();

            var differences = Difference<Car>.Between(first, second).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(second);
            differences.First().PropertyName.Should().Be(string.Empty);
            differences.First().PropertyType.Should().Be(typeof(Car));
            differences.First().ObjectType.Should().Be(typeof(Car));
        }
        
        [Test]
        public void Between_ComplexTypeSameReference_ResultShouldNoDifference()
        {
            var car = _fixture.Create<Car>();

            var differences = Difference<Car>.Between(car, car).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void Between_ComplexIEquatableDifferentReferenceSameValues_DifferencesShouldHaveNoItems()
        {
            var first = new Car
            {
                Make = "Make",
                Model = "Model",
                Year = 2000,
                Sold = DateTime.Today,
                Mileage = 60000
            };
            
            var second = new Car
            {
                Make = "Make",
                Model = "Model",
                Year = 2000,
                Sold = DateTime.Today,
                Mileage = 60000
            };

            var differences = Difference<Car>.Between(first, second).ToList();

            differences.Should().HaveCount(0);
        }
        
    }
}