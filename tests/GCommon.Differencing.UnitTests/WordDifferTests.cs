using System;
using System.Linq;
using FluentAssertions;
using GCommon.Differencing.Differentiators;
using NUnit.Framework;

namespace GCommon.Differencing.UnitTests
{
    [TestFixture]
    public class WordDifferTests
    {
        [Test]
        public void DifferenceIn_FirstSecond_ShouldHaveExpectedCount()
        {
            var differ = new WordDiffer(StringComparison.OrdinalIgnoreCase);

            var results = differ.DifferenceIn("first", "second");

            results.Should().HaveCount(6);
        }
        
        [Test]
        public void DifferenceIn_CatBat_ShouldHaveExpectedCount()
        {
            var differ = new WordDiffer(StringComparison.OrdinalIgnoreCase);

            var results = differ.DifferenceIn("cat", "bat").ToList();

            results.Should().HaveCount(1);
            results.First().Left.Should().Be('c');
            results.First().Right.Should().Be('b');
            results.First().PropertyType.Should().Be(typeof(char?));
            results.First().DifferenceType.Should().Be(DifferenceType.Changed);
        }
    }
}