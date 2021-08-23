using System.Linq;
using AutoFixture;
using FluentAssertions;
using GCommon.Differencing.UnitTests.TestClasses;
using NUnit.Framework;

namespace GCommon.Differencing.UnitTests
{
    [TestFixture]
    public class ExtensionsTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
        
        [Test]
        public void DiffersBy_SimpleStringDifferent_ShouldHaveSingleDifference()
        {
            var first = "Test1";
            var second = "Test2";

            var result = first.DiffersFrom(second);

            result.Should().HaveCount(1);
        }
        
        [Test]
        public void DiffersBy_ComplexTypeDifferent_ShouldHaveAllPropertiesDifferent()
        {
            var first = _fixture.Create<Car>();
            var second = _fixture.Create<Car>();

            var result = first.DiffersFrom(second);

            result.Should().HaveCount(5);
        }

        [Test]
        public void SequenceDiffersBy_SimpleSameOrdered_ResultShouldHaveZeroDifferences()
        {
            var first = new[] {"A", "B", "C"};
            var second = new[] {"A", "B", "C"};

            var result = first.SequenceDiffersBy(second).ToList();

            result.Should().HaveCount(0);
        }
        
        [Test]
        public void SequenceDiffersBy_SimpleSameUnordered_ResultShouldHaveZeroDifferences()
        {
            var first = new[] {"A", "B", "C"};
            var second = new[] {"A", "C", "B"};

            var result = first.SequenceDiffersBy(second).ToList();

            result.Should().HaveCount(0);
        }
        
        [Test]
        public void SequenceDiffersBy_SimpleDifferent_ResultShouldHaveOneDifference()
        {
            var first = new[] {"A", "B", "C"};
            var second = new[] {"D", "E", "F"};

            var result = first.SequenceDiffersBy(second).ToList();

            result.Should().HaveCount(6);
        }
        
        [Test]
        public void SequenceDiffersBy_FirstHasOneMore_ResultShouldHaveOneDifference()
        {
            var first = new[] {"A", "B", "C", "D"};
            var second = new[] {"A", "C", "B"};

            var result = first.SequenceDiffersBy(second).ToList();

            result.Should().HaveCount(1);
            result.First().Left.Should().Be("D");
        }
        
        [Test]
        public void SequenceDiffersBy_SecondHasOneMore_ResultShouldHaveOneDifference()
        {
            var first = new[] {"A", "B", "C"};
            var second = new[] {"A", "C", "B", "D"};

            var result = first.SequenceDiffersBy(second).ToList();

            result.Should().HaveCount(1);
            result.First().Right.Should().Be("D");
        }
        
        [Test]
        public void SequenceDiffersBy_ComplexCollection_ResultShouldHaveExpectedDifferences()
        {
            var first = _fixture.CreateMany<Car>();
            var second = _fixture.CreateMany<Car>();

            var result = first.SequenceDiffersBy(second, c => c.Make).ToList();

            result.Should().HaveCount(10);
        }
    }
}