using AutoFixture;
using FluentAssertions;
using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;
using GCommon.Differencing.UnitTests.TestClasses;
using NUnit.Framework;

namespace GCommon.Differencing.UnitTests
{
    [TestFixture]
    public class CollectionDifferTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
        [Test]
        public void DifferenceIn_IntegersSortedCompare_ShouldHaveExpectedCount()
        {
            var first = _fixture.CreateMany<int>();
            var second = _fixture.CreateMany<int>();
            
            var differ = new CollectionDiffer<int>();
            
            var results = differ.DifferenceIn(first, second, i => i, CollectionMatchMode.Sort);

            results.Should().HaveCount(3);
        }

        [Test]
        public void DifferenceIn_IntegersFirstHasMoreSortCompare_ShouldHaveExpectedCount()
        {
            var first = _fixture.CreateMany<int>(5);
            var second = _fixture.CreateMany<int>();
            
            var differ = new CollectionDiffer<int>();
            
            var results = differ.DifferenceIn(first, second, i => i, CollectionMatchMode.Sort);

            results.Should().HaveCount(5);
        }
        
        [Test]
        public void DifferenceIn_IntegersSecondHasMoreSortCompare_ShouldHaveExpectedCount()
        {
            var first = _fixture.CreateMany<int>();
            var second = _fixture.CreateMany<int>(5);
            
            var differ = new CollectionDiffer<int>();
            
            var results = differ.DifferenceIn(first, second, i => i, CollectionMatchMode.Sort);

            results.Should().HaveCount(5);
        }
        
        [Test]
        public void DifferenceIn_NullableIntegersSecondHasMoreSortCompare_ShouldHaveExpectedCount()
        {
            var first = _fixture.CreateMany<int?>(13);
            var second = _fixture.CreateMany<int?>(5);
            
            var differ = new CollectionDiffer<int?>();
            
            var results = differ.DifferenceIn(first, second, i => i, CollectionMatchMode.Sort);

            results.Should().HaveCount(13);
        }
        
        [Test]
        public void DifferenceIn_ComplexSecondHasMoreSortCompare_ShouldHaveExpectedCount()
        {
            var first = _fixture.CreateMany<Car>();
            var second = _fixture.CreateMany<Car>(5);
            
            var differ = new CollectionDiffer<Car>();
            
            var results = differ.DifferenceIn(first, second, c => c.Model, CollectionMatchMode.Sort);

            results.Should().HaveCount(5);
        }
    }
}