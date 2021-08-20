using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using GCommon.Differencing.UnitTests.TestClasses;
using NUnit.Framework;

namespace GCommon.Differencing.UnitTests
{
    [TestFixture]
    public class DifferentiableTests
    {
        [Test]
        public void DifferenceFrom_AllPropertiesDifferent_ShouldHaveCorrectCount()
        {
            var fixture = new Fixture();
            var car1 = fixture.Create<Car>();
            var car2 = fixture.Create<Car>();

            var result = car1.DiffersFrom(car2);

            result.Should().HaveCount(5);
        }
        
        [Test]
        public void Diff_SomePropertiesDifferent_ShouldHaveCorrectCount()
        {
            var fixture = new Fixture();
            fixture.Customize<Car>(c => c.Without(p => p.Model));
            var car1 = fixture.Create<Car>();
            var car2 = fixture.Create<Car>();

            var result = car1.DiffersFrom(car2);

            result.Should().HaveCount(4);
        }

        [Test]
        public void Diff_AllPropertiesDifference_ShouldHaveCorrectCount()
        {
            var fixture = new Fixture();
            var owner1 = fixture.Create<Owner>();
            var owner2 = fixture.Create<Owner>();

            var results = owner1.DiffersFrom(owner2);

            results.Should().HaveCountGreaterThan(4);
        }

        [Test]
        public void Equals_OnlyDifferentCarProperties_ReturnsTrue()
        {
            var fixture = new Fixture();
            
            fixture.Customize<Owner>(c => c.Without(p => p.Cars));
            var owner1 = new Owner
            {
                FirstName = "John",
                LastName = "Smith",
                Age = 50
            };
            var owner2 = new Owner
            {
                FirstName = "John",
                LastName = "Smith",
                Age = 50
            };
            
            var car1 = fixture.Create<Car>();
            var car2 = fixture.Create<Car>();
            
            owner1.Cars = new List<Car> {car1};
            owner2.Cars = new List<Car> {car2};

            var result = owner1.Equals(owner2);

            result.Should().BeFalse();
        }
    }
}