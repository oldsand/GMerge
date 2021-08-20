using AutoFixture;
using FluentAssertions;
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
        public void Between_SimpleTypeDiffers_ResultShouldHaveSingleCount()
        {
            var v1 = _fixture.Create<int>();
            var v2 = _fixture.Create<int>();

            var result = Difference.Between(v1, v2);

            result.Should().HaveCount(1);
        }
        
        [Test]
        public void Between_Collections_ResultShouldContainExpectedCount()
        {
            var v1 = new[] {"A", "B", "C"};
            var v2 = new[] {"A", "C", "B"};

            var result = Difference.BetweenCollection(v1, v2);

            result.Should().HaveCount(0);
        }
        
        [Test]
        public void BetweenCollections_FirstHasOneMore_ResultShouldHaveOneDifference()
        {
            var first = new[] {"A", "B", "C", "D"};
            var second = new[] {"A", "C", "B"};

            var result = Difference.BetweenCollection(first, second);

            result.Should().HaveCount(1);
        }
        
        [Test]
        public void BetweenCollections_SecondHasOneMore_ResultShouldHaveOneDifference()
        {
            var first = new[] {"A", "B", "C"};
            var second = new[] {"A", "C", "B", "D"};

            var result = Difference.BetweenCollection(first, second);

            result.Should().HaveCount(1);
        }
    }
}