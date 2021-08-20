using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GCommon.Core.Extensions;
using NUnit.Framework;

namespace GCommon.Core.UnitTests
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void LeftOuterJoin_WhenCalled_ShouldHaveResult()
        {
            var first = new[] {"A", "B", "C", "D", "E"}.AsEnumerable();
            var second = new[] {"A", "A", "C", "B"}.AsEnumerable();

            var result = first.LeftOuterJoin(second, s => s, s => s, (s1, s2) => new
            {
                Fisrt = s1,
                Second = s2
            }).ToList();

            result.Should().HaveCount(6);
        }
        
        [Test]
        public void RightOuterJoin_WhenCalled_ShouldHaveResult()
        {
            var first = new[] {"A", "B", "C", "D", "E"}.AsEnumerable();
            var second = new[] {"A", "A", "C", "B", "F"}.AsEnumerable();

            var result = first.RightOuterJoin(second, s => s, s => s, (s1, s2) => new
            {
                Fisrt = s1,
                Second = s2
            }).ToList();

            result.Should().HaveCount(5);
        }
        
        [Test]
        public void RightAntiSemiJoin_WhenCalled_ShouldHaveResult()
        {
            var first = new[] {"A", "B", "C"}.AsEnumerable();
            var second = new[] {"A", "A", "C", "B", "G"}.AsEnumerable();

            var result = first.RightAntiSemiJoin(second, s => s, s => s, (s1, s2) => new
            {
                Fisrt = s1,
                Second = s2
            }).ToList();

            result.Should().HaveCount(1);
        }
        
        [Test]
        public void FullOuterJoin_WhenCalled_ShouldHaveResult()
        {
            var first = new[] {"A", "B", "C", "D", "E"}.AsEnumerable();
            var second = new[] {"A", "A", "C", "B"}.AsEnumerable();

            var result = first.FullOuterJoin(second, s => s, s => s, (s1, s2) => new
            {
                Fisrt = s1,
                Second = s2
            }).ToList();

            result.Should().HaveCount(6);
        }

        [Test]
        public void FullOuterJoin_LargeCollection_ShouldHaveExpectedCount()
        {
            var fixture = new Fixture();
            var first = fixture.CreateMany<string>(10000);
            var second = fixture.CreateMany<string>(10000);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var result = first.FullOuterJoin(second, i => i, i => i, (i1, i2) => new
            {
                First = i1,
                Second = i2
            }).ToList();
            
            stopWatch.Stop();

            result.Should().HaveCount(20000);
        }
    }
}