using System;
using System.Collections.Generic;
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

            var difference = Difference.Create(first, second);

            difference.Left.Should().Be(first);
            difference.Right.Should().Be(second);
            difference.PropertyType.Should().Be(typeof(string));
            difference.ObjectType.Should().Be(null);
            difference.PropertyName.Should().Be(null);
        }
        
        [Test]
        public void Create_PropertyExpressionSimpleDateTime_ResultShouldHaveExpectedData()
        {
            var first = _fixture.Create<DateTime>();
            var second = _fixture.Create<DateTime>();

            var difference = Difference.Create(first, second);

            difference.Left.Should().Be(first);
            difference.Right.Should().Be(second);
            difference.PropertyType.Should().Be(typeof(DateTime));
            difference.ObjectType.Should().Be(null);
            difference.PropertyName.Should().Be(null);
        }
        
        [Test]
        public void Create_PropertyExpressionComplexType_ResultShouldHaveExpectedData()
        {
            var first = _fixture.Create<Car>();
            var second = _fixture.Create<Car>();

            var difference = Difference.Create(first.Model, second.Model, nameof(first.Model), typeof(Car));

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

            var result = Difference.Between(first, second);

            result.Should().HaveCount(1);
        }
        
        [Test]
        public void Between_SimpleStringTypeDiffers_ResultShouldHaveSingleCount()
        {
            var first = _fixture.Create<string>();
            var second = _fixture.Create<string>();

            var result = Difference.Between(first, second);

            result.Should().HaveCount(1);
        }

        [Test]
        public void Between_ComplexTypeDiffers_ResultShouldHaveSingleCount()
        {
            var first = _fixture.Create<Car>();
            var second = _fixture.Create<Car>();

            var differences = Difference.Between(first, second).ToList();

            differences.Should().HaveCount(1);
        }
        
        [Test]
        public void Between_ComplexTypeSameReference_ResultShouldNoDifference()
        {
            var car = _fixture.Create<Car>();

            var differences = Difference.Between(car, car).ToList();

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

            var differences = Difference.Between(first, second).ToList();

            differences.Should().HaveCount(0);
        }

        [Test]
        public void Between_PropertyExpressionToSimpleType_ShouldHaveExpectedDifference()
        {
            var first = _fixture.Create<Car>();
            var second = _fixture.Create<Car>();

            var differences = Difference.Between(first, second, c => c.VinNumber).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first.VinNumber);
            differences.First().Right.Should().Be(second.VinNumber);
            differences.First().PropertyType.Should().Be(typeof(int));
            differences.First().PropertyName.Should().Be(nameof(first.VinNumber));
            differences.First().ObjectType.Should().Be(typeof(Car));
            differences.First().DifferenceType.Should().Be(DifferenceType.Changed);
        }
        
        [Test]
        public void Between_PropertyExpressionToComplexTypeAreDifferent_ShouldHaveExpectedDifference()
        {
            var first = _fixture.Create<CarTitle>();
            var second = _fixture.Create<CarTitle>();

            var differences = Difference.Between(first, second, c => c.Car).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first.Car);
            differences.First().Right.Should().Be(second.Car);
            differences.First().PropertyType.Should().Be(typeof(Car));
            differences.First().PropertyName.Should().Be(nameof(first.Car));
            differences.First().ObjectType.Should().Be(typeof(CarTitle));
            differences.First().DifferenceType.Should().Be(DifferenceType.Changed);
        }
        
        [Test]
        public void Between_PropertyExpressionToComplexTypeAreSame_ShouldHaveExpectedDifference()
        {
            var first = _fixture.Create<CarTitle>();

            var differences = Difference.Between(first, first, c => c.Car).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void Between_PropertyExpressionToComplexTypeAreEqual_ShouldHaveExpectedDifference()
        {
            var car1 = new Car
            {
                VinNumber = 123
            };
            var car2 = new Car
            {
                VinNumber = 123
            };
            var first = _fixture.Build<CarTitle>().With(x => x.Car, car1).Create();
            var second = _fixture.Build<CarTitle>().With(x => x.Car, car2).Create();

            var differences = Difference.Between(first, second, c => c.Car).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void Between_PropertyExpressionToCollection_ShouldHaveExpectedDifference()
        {
            var first = _fixture.Create<Owner>();
            var second = _fixture.Create<Owner>();

            var differences = Difference.Between(first, second, c => c.Cars).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first.Cars);
            differences.First().Right.Should().Be(second.Cars);
            differences.First().PropertyType.Should().Be(typeof(IEnumerable<Car>));
            differences.First().PropertyName.Should().Be(nameof(first.Cars));
            differences.First().ObjectType.Should().Be(typeof(Owner));
            differences.First().DifferenceType.Should().Be(DifferenceType.Changed);
        }
        
        
        [Test]
        public void BetweenSequence_OfTypeThatImplementsIEquatable_ShouldHaveExpectedDifference()
        {
            var first = _fixture.CreateMany<Car>().ToList();
            var second = _fixture.CreateMany<Car>().ToList();

            var differences = Difference.BetweenSequence(first, second).ToList();

            differences.Should().HaveCount(3);
        }
        
    }
}