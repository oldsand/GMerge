using System;
using System.Linq;
using FluentAssertions;
using GCommon.Core.Helpers;
using NUnit.Framework;

namespace GCommon.Core.UnitTests.HelperTests
{
    [TestFixture]
    public class HexTests
    {
        [Test]
        public void Constructor_ValidHex_ShouldNotBeNull()
        {
            const string value = "FF00";
            
            var hex = new Hex(value);

            hex.Should().NotBeNull();
        }
        
        [Test]
        public void Constructor_ValidHex_ShouldHaveValue()
        {
            const string value = "15A7BE";

            var hex = new Hex(value);
            
            hex.Value.Should().Be(value);
        }
        
        [Test]
        public void Constructor_InvalidHex_ShouldThrowArgumentException()
        {
            const string value = "0F4EH9";
            
            FluentActions.Invoking(() => new Hex(value)).Should().Throw<ArgumentException>()
                .WithMessage($"Value '{value}' is not valid hex");
        }
        
        [Test]
        public void Empty_InvalidHex_ShouldThrowArgumentException()
        {
            var hex = Hex.Empty;

            hex.Should().NotBeNull();
            hex.Value.Should().Be(string.Empty);
        }
        
        [Test]
        public void Reverse_EvenNumber_ShouldReturnReversedValue()
        {
            const string value = "FF00";
            
            var hex = new Hex(value).Reverse();

            hex.Value.Should().Be("00FF");
        }
        
        [Test]
        public void Reverse_EvenNumberLongerData_ShouldReturnReversedValue()
        {
            const string value = "0123456789ABCDEF";
            
            var hex = new Hex(value).Reverse();

            hex.Value.Should().Be("EFCDAB8967452301");
        }

        [Test]
        public void Reverse_OddNumber_ShouldReturnReversedValue()
        {
            const string value = "F00";
            
            var hex = new Hex(value).Reverse();

            hex.Value.Should().Be("00F");
        }

        [Test]
        public void Head_ValidLength_ShouldBeExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Head(2);

            result.Value.Should().Be("12");
        }
        
        [Test]
        public void Head_ZeroLength_ShouldBeEqualToEmpty()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Head(0);
            
            result.Should().Be(Hex.Empty);
        }
        
        [Test]
        public void Head_EqualLengthValue_ShouldBeEqualToOriginal()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Head(8);
            
            result.Should().Be(hex);
        }
        
        [Test]
        public void Head_LargerLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Head(12)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void Head_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Head(-1)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void Tail_ValidLength_ShouldBeExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Tail(2);

            result.Value.Should().Be("CD");
        }
        
        [Test]
        public void Tail_ZeroLength_ShouldBeEqualToEmpty()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Tail(0);
            
            result.Should().Be(Hex.Empty);
        }
        
        [Test]
        public void Tail_EqualLengthValue_ShouldBeEqualToOriginal()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Tail(8);
            
            result.Should().Be(hex);
        }
        
        [Test]
        public void Tail_LargerLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Tail(12)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void Tail_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Tail(-1)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void DropHead_ValidLength_ShouldBeExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.DropHead(2);

            result.Value.Should().Be("34ABCD");
        }
        
        [Test]
        public void DropHead_ZeroLength_ShouldBeEqualToOriginal()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.DropHead(0);
            
            result.Should().Be(hex);
        }
        
        [Test]
        public void DropHead_EqualLengthValue_ShouldBeEqualToEmpty()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.DropHead(8);
            
            result.Should().Be(Hex.Empty);
        }
        
        [Test]
        public void DropHead_LargerLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropHead(12)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void DropHead_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropHead(-1)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void DropTail_ValidLength_ShouldBeExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.DropTail(2);

            result.Value.Should().Be("1234AB");
        }
        
        [Test]
        public void DropTail_ZeroLength_ShouldBeEqualToOriginal()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.DropTail(0);
            
            result.Should().Be(hex);
        }
        
        [Test]
        public void DropTail_EqualLengthValue_ShouldBeEqualToEmpty()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.DropTail(8);
            
            result.Should().Be(Hex.Empty);
        }
        
        [Test]
        public void DropTail_LargerLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropTail(12)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void DropTail_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropTail(-1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ToEnumerable_ValidLength_ShouldHaveExpectedCount()
        {
            var hex = new Hex("0034001500F4009C");

            var results = hex.ToEnumerable(4).ToArray();

            results.Should().HaveCount(4);
            results[0].Value.Should().Be("0034");
            results[1].Value.Should().Be("0015");
            results[2].Value.Should().Be("00F4");
            results[3].Value.Should().Be("009C");
        }

        [Test]
        public void Consume_ValidLength_ShouldHaveExpectedResult()
        {
            var hex = new Hex("0034001500F4009C");

            var result = hex.Consume(8);

            result.Value.Should().Be("00340015");
        }
        
        [Test]
        public void Consume_ValidLength_OriginalShouldHaveRemovedData()
        {
            var hex = new Hex("0034001500F4009C");

            var result = hex.Consume(8);

            result.Should().NotBeNull();
            hex.Length.Should().Be(8);
            hex.Value.Should().Be("00F4009C");
        }

        [Test]
        public void Consume_ZeroLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            var result = hex.Consume(0);

            result.Should().NotBeNull();
            result.Value.Should().Be(string.Empty);
            result.Length.Should().Be(0);
        }
        
        [Test]
        public void Consume_LargerLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Consume(12)).Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void Consume_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Consume(-1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ImplicitEquals_WithString_ShouldBeTrue()
        {
            var hex = new Hex("AB");

            var result = hex == "AB";

            result.Should().BeTrue();
        }

        [Test]
        public void ToBool_FF_ShouldBeTrue()
        {
            var hex = new Hex("FF");

            var result = hex.ToBool();
            
            result.Should().BeTrue();
        }
        
        [Test]
        public void ToBool_00_ShouldBeTrue()
        {
            var hex = new Hex("00");

            var result = hex.ToBool();
            
            result.Should().BeFalse();
        }
        
        [Test]
        public void ToInt_00FF_ShouldBeExpectedValue()
        {
            var hex = new Hex("FF");

            var result = hex.ToInt();

            result.Should().Be(255);
        }

        [Test]
        public void ToTimeSpan_SomeData_ShouldBeExpectedTimeSpan()
        {
            var hex = new Hex("FF");

            var result = hex.ToTimeSpan();
        }
    }
}