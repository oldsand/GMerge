using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GCommon.Differencing.Differentiators;
using GCommon.Differencing.UnitTests.TestClasses;
using NUnit.Framework;

namespace GCommon.Differencing.UnitTests
{
    [TestFixture]
    public class GenericDifferTests
    {
        [Test]
        public void DifferenceIn_IntegersDifferent_ShouldHaveSingleCount()
        {
            var differ = new GenericDiffer<int>();

            var differences = differ.DifferenceIn(1, 2);

            differences.Should().HaveCount(1);
        }
        
        [Test]
        public void DifferenceIn_IntegersDifferent_ShouldHaveLeftRightValues()
        {
            var differ = new GenericDiffer<int>();

            var differences = differ.DifferenceIn(1, 2).ToList();

            differences.First().Left.Should().Be(1);
            differences.First().Right.Should().Be(2);
        }
        
        [Test]
        public void DifferenceIn_TypeThatImplementsIDifferentiable_ShouldHaveExpectedCount()
        {
            var differ = new GenericDiffer<Car>();
            var fixture = new Fixture();
            var first = fixture.Create<Car>();
            var second = fixture.Create<Car>();

            var differences = differ.DifferenceIn(first, second);

            //five properties should result in five differences
            differences.Should().HaveCount(5);
        }
        
        [Test]
        public void DifferenceIn_TypeThatDoesNotImplementsIDifferentiable_ShouldHaveSingleCount()
        {
            var differ = new GenericDiffer<CarTitle>();
            var fixture = new Fixture();
            var first = fixture.Create<CarTitle>();
            var second = fixture.Create<CarTitle>();

            var differences = differ.DifferenceIn(first, second);
            
            differences.Should().HaveCount(1);
        }
        
        [Test]
        public void DifferenceIn_TypeThatDoesNotImplementsIDifferentiableAndAreSame_ShouldHaveNoCount()
        {
            var differ = new GenericDiffer<CarTitle>();
            var fixture = new Fixture();
            var carTitle = fixture.Create<CarTitle>();

            var differences = differ.DifferenceIn(carTitle, carTitle);
            
            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_TypeThatDoesNotImplementsIDifferentiableAndAreEqualButDifferentReference_ShouldHaveSingleCount()
        {
            var differ = new GenericDiffer<CarTitle>();
            
            var car = new Car
            {
                Make = "Make",
                Model = "Model",
                Mileage = 100,
                Sold = DateTime.Today,
                Year = 1999
            };

            var owner = new Owner
            {
                FirstName = "FirstName",
                LastName = "FirstName",
                Age = 33
            };
            
            var first = new CarTitle
            {
                Id = 23,
                Created = DateTime.Today,
                Car = car,
                Owner = owner
            };
            
            var second = new CarTitle
            {
                Id = 23,
                Created = DateTime.Today,
                Car = car,
                Owner = owner
            };

            var differences = differ.DifferenceIn(first, second);
            
            differences.Should().HaveCount(1);
        }
    }
}