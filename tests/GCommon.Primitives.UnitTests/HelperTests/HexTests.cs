using System;
using System.Linq;
using FluentAssertions;
using GCommon.Primitives.Helpers;
using NUnit.Framework;

namespace GCommon.Primitives.UnitTests.HelperTests
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
            hex.Value.Should().Be("00");
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
        public void Head_ValidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var head = hex.Head(2);

            head.Value.Should().Be("12");
        }
        
        [Test]
        public void Head_ZeroLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Head(0)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be greater than zero and less than the length of the hex value");
        }
        
        [Test]
        public void Head_InvalidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Head(12)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be greater than zero and less than the length of the hex value");
        }
        
        [Test]
        public void Tail_ValidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var head = hex.Tail(2);

            head.Value.Should().Be("CD");
        }
        
        [Test]
        public void Tail_ZeroLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Tail(0)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be greater than zero and less than the length of the hex value");
        }
        
        [Test]
        public void Tail_InvalidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.Tail(12)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be greater than zero and less than the length of the hex value");
        }
        
        [Test]
        public void DropHead_ValidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var head = hex.DropHead(2);

            head.Value.Should().Be("34ABCD");
        }
        
        [Test]
        public void DropHead_LengthOfValue_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropHead(8)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be less than the length of the hex value");
        }
        
        [Test]
        public void DropHead_InvalidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropHead(12)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be less than the length of the hex value");
        }
        
        [Test]
        public void DropTail_ValidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            var head = hex.DropTail(2);

            head.Value.Should().Be("1234AB");
        }
        
        [Test]
        public void DropTail_LengthOfValue_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropTail(8)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be less than the length of the hex value");
        }
        
        [Test]
        public void DropTail_InvalidLength_ReturnsExpectedValue()
        {
            var hex = new Hex("1234ABCD");

            FluentActions.Invoking(() => hex.DropTail(12)).Should().Throw<ArgumentException>()
                .WithMessage("Length must be less than the length of the hex value");
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
    }
}