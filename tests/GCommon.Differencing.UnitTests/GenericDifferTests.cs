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
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
        
        [Test]
        public void DifferenceIn_SecondNull_ShouldHaveSingleCountAndExpectedProperties()
        {
            var first = _fixture.Create<bool>();
            var differ = new GenericDiffer<object>();

            var differences = differ.DifferenceIn(first, null).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(null);
            differences.First().PropertyType.Should().Be(typeof(object));
            differences.First().PropertyName.Should().Be(null);
            differences.First().ObjectType.Should().Be(null);
            differences.First().DifferenceType.Should().Be(DifferenceType.Removed);
        }
        
        [Test]
        public void DifferenceIn_FirstNull_ShouldHaveSingleCountAndExpectedProperties()
        {
            var value = _fixture.Create<bool>();
            var differ = new GenericDiffer<object>();

            var differences = differ.DifferenceIn(null, value).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(null);
            differences.First().Right.Should().Be(value);
            differences.First().PropertyType.Should().Be(typeof(object));
            differences.First().PropertyName.Should().Be(null);
            differences.First().ObjectType.Should().Be(null);
            differences.First().DifferenceType.Should().Be(DifferenceType.Added);
        }
        
        [Test]
        public void DifferenceIn_BothNull_ShouldHaveNoDifference()
        {
            var differ = new GenericDiffer<object>();

            var differences = differ.DifferenceIn(null, null).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_BoolDifferent_ShouldHaveSingleCountAndExpectedProperties()
        {
            var first = _fixture.Create<bool>();
            var second = _fixture.Create<bool>();
            var differ = new GenericDiffer<bool>();

            var differences = differ.DifferenceIn(first, second).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(second);
            differences.First().PropertyType.Should().Be(typeof(bool));
            differences.First().PropertyName.Should().Be(null);
            differences.First().ObjectType.Should().Be(null);
        }
        
        [Test]
        public void DifferenceIn_BoolSame_ShouldHaveNoDifferences()
        {
            var value = _fixture.Create<bool>();
            var differ = new GenericDiffer<bool>();

            var differences = differ.DifferenceIn(value, value).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_IntegersDifferent_ShouldHaveSingleCountAndExpectedProperties()
        {
            var first = _fixture.Create<int>();
            var second = _fixture.Create<int>();
            var differ = new GenericDiffer<int>();

            var differences = differ.DifferenceIn(first, second).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(second);
            differences.First().PropertyType.Should().Be(typeof(int));
            differences.First().PropertyName.Should().Be(null);
            differences.First().ObjectType.Should().Be(null);
        }
        
        [Test]
        public void DifferenceIn_IntegersSame_ShouldHaveNoDifferences()
        {
            var value = _fixture.Create<int>();
            var differ = new GenericDiffer<int>();

            var differences = differ.DifferenceIn(value, value).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_DoubleDifferent_ShouldHaveSingleCountAndExpectedProperties()
        {
            var first = _fixture.Create<double>();
            var second = _fixture.Create<double>();
            var differ = new GenericDiffer<double>();

            var differences = differ.DifferenceIn(first, second).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(second);
            differences.First().PropertyType.Should().Be(typeof(double));
            differences.First().PropertyName.Should().Be(null);
            differences.First().ObjectType.Should().Be(null);
        }
        
        [Test]
        public void DifferenceIn_DoubleSame_ShouldHaveNoDifferences()
        {
            var value = _fixture.Create<double>();
            var differ = new GenericDiffer<double>();

            var differences = differ.DifferenceIn(value, value).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_StringDifferent_ShouldHaveSingleCountAndExpectedProperties()
        {
            var first = _fixture.Create<string>();
            var second = _fixture.Create<string>();
            var differ = new GenericDiffer<string>();

            var differences = differ.DifferenceIn(first, second).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(second);
            differences.First().PropertyType.Should().Be(typeof(string));
            differences.First().PropertyName.Should().Be(null);
            differences.First().ObjectType.Should().Be(null);
        }
        
        [Test]
        public void DifferenceIn_StringSame_ShouldHaveNoDifferences()
        {
            var value = _fixture.Create<string>();
            var differ = new GenericDiffer<string>();

            var differences = differ.DifferenceIn(value, value).ToList();

            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_StringDifferentWithPropertyName_ShouldHaveSingleCountAndExpectedProperties()
        {
            var first = _fixture.Create<string>();
            var second = _fixture.Create<string>();
            var differ = new GenericDiffer<string>("String", typeof(string));

            var differences = differ.DifferenceIn(first, second).ToList();

            differences.Should().HaveCount(1);
            differences.First().Left.Should().Be(first);
            differences.First().Right.Should().Be(second);
            differences.First().PropertyType.Should().Be(typeof(string));
            differences.First().PropertyName.Should().Be("String");
            differences.First().ObjectType.Should().Be(typeof(string));
        }

        [Test]
        public void Equals_ComplexTypeThatImplementsIEquatableDifferent_ShouldNotBeEqual()
        {
            var differ = new GenericDiffer<Car>();
            var fixture = new Fixture();
            var first = fixture.Create<Car>();
            var second = fixture.Create<Car>();

            var result = differ.Equals(first, second);
            
            result.Should().BeFalse();
        }
        
        [Test]
        public void Equals_ComplexTypeThatImplementsIEquatableAreEqual_ShouldBeTrue()
        {
            var differ = new GenericDiffer<Car>();
            
            var first = new Car
            {
                VinNumber = 1,
                Make = "Honda",
                Model = "Civic",
                Mileage = 60000,
                Sold = DateTime.Today,
                Year = 2013
            };
            
            var second = new Car
            {
                VinNumber = 1,
                Make = "Honda",
                Model = "Civic",
                Mileage = 60000,
                Sold = DateTime.Today,
                Year = 2013
            };
            

            var result = differ.Equals(first, second);
            
            result.Should().BeTrue();
        }
        
        [Test]
        public void DifferenceIn_ComplexTypeThatImplementsIEquatableAreEqual_ShouldNoDifference()
        {
            var differ = new GenericDiffer<Car>();
            
            var first = new Car
            {
                VinNumber = 1,
                Make = "Honda",
                Model = "Civic",
                Mileage = 60000,
                Sold = DateTime.Today,
                Year = 2013
            };
            
            var second = new Car
            {
                VinNumber = 1,
                Make = "Honda",
                Model = "Civic",
                Mileage = 60000,
                Sold = DateTime.Today,
                Year = 2013
            };
            

            var differences = differ.DifferenceIn(first, second);
            
            differences.Should().HaveCount(0);
        }
        
        [Test]
        public void DifferenceIn_ComplexTypeThatImplementsIEquatableDifferent_ShouldHaveExpectedCount()
        {
            var differ = new GenericDiffer<Car>();
            var fixture = new Fixture();
            var first = fixture.Create<Car>();
            var second = fixture.Create<Car>();

            var differences = differ.DifferenceIn(first, second);
            
            differences.Should().HaveCount(1);
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
        public void Equal_ComplexTypeThatDoesNotImplementsIEquatableAreEqual_ShouldBeFalse()
        {
            var differ = new GenericDiffer<CarTitle>();
            
            var first = new CarTitle
            {
                Id = 1,
                Created = DateTime.Today
            };
            
            var second = new CarTitle
            {
                Id = 1,
                Created = DateTime.Today
            };


            var result = differ.Equals(first, second);
            
            result.Should().BeFalse();
        }
        
        [Test]
        public void DifferenceIn_ComplexTypeThatDoesNotImplementsIEquatableAreEqual_ShouldHaveSingleCount()
        {
            var differ = new GenericDiffer<CarTitle>();
            
            var first = new CarTitle
            {
                Id = 1,
                Created = DateTime.Today
            };
            
            var second = new CarTitle
            {
                Id = 1,
                Created = DateTime.Today
            };


            var differences = differ.DifferenceIn(first, second);
            
            differences.Should().HaveCount(1);
        }

        [Test]
        public void DifferenceIn_ComplexTypeThatDoesNotImplementsIEquatableAreSame_ShouldHaveNoCount()
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