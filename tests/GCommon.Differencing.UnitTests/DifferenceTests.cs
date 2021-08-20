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
    }
}