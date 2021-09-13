using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GCommon.Core.Helpers
{
    public class Hex : IEquatable<Hex>
    {
        private const int ByteLength = 2;
        
        public Hex(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!IsValidHex(value))
                throw new ArgumentException($"Value '{value}' is not valid hex");
            
            Value = value;
        }
        
        public string Value { get; private set; }

        public int Length => Value.Length;
        
        public static Hex Empty => new Hex(string.Empty);

        public static implicit operator Hex(string value)
        {
            return new Hex(value);
        } 

        public static implicit operator string(Hex hex)
        {
            return hex.Value;
        }

        /// <summary>
        /// Gets the specified index of the current hex value
        /// </summary>
        /// <param name="index">Index of the item to retrieve</param>
        public Hex this[int index]
        {
            get
            {
                ValidateIndex(index);

                return new Hex(Value[index].ToString());
            }
        }

        /// <summary>
        /// Determines whether the specified string is valid hex data 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True if the string is empty, the string contains only hex characters, or if the
        /// string starts with the hex specifier and contains only hex characters</returns>
        /// <remarks>Valid hex character are combinations 0-9, a-f, or A-F]</remarks>
        public static bool IsValid(string input)
        {
            return IsValidHex(input);
        }

        /// <summary>
        /// Converts the current hex value to a <c>bool</c>
        /// </summary>
        /// <returns><c>bool</c></returns>
        public bool ToBool()
        {
            int.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            return result > 0;
        }

        /// <summary>
        /// Converts the current hex value to a <c>int</c>
        /// </summary>
        /// <returns><c>int</c></returns>
        public int ToInt()
        {
            int.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            return result;
        }
        
        /// <summary>
        /// Converts the current hex value to a <c>byte</c>
        /// </summary>
        /// <returns><c>int</c></returns>
        public byte ToByte()
        {
            byte.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            return result;
        }

        /// <summary>
        /// Converts the current hex value to a <c>float</c>
        /// </summary>
        /// <returns><c>float</c></returns>
        public float ToFloat()
        {
            int.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            var bytes = BitConverter.GetBytes(result);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Converts the current hex value to a <c>double</c>
        /// </summary>
        /// <returns><c>double</c></returns>
        public double ToDouble()
        {
            long.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            var bytes = BitConverter.GetBytes(result);
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// Converts the current hex value to a <c>char</c>
        /// </summary>
        /// <returns><c>char</c></returns>
        public string ToChar()
        {
            int.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            return char.ConvertFromUtf32(result);
        }

        /// <summary>
        /// Converts the current hex value to a <c>DateTime</c>
        /// </summary>
        /// <returns><c>DateTime</c></returns>
        public DateTime ToDateTime()
        {
            long.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            return DateTime.FromBinary(result).AddYears(1600).ToLocalTime();
        }

        /// <summary>
        /// Converts the current hex value to a <c>TimeSpan</c>
        /// </summary>
        /// <returns><c>TimeSpan</c></returns>
        public TimeSpan ToTimeSpan()
        {
            long.TryParse(Value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result);
            return TimeSpan.FromTicks(result);
        }

        /// <summary>
        /// Forms an enumerable of hex on the current hex value
        /// </summary>
        /// <param name="elementLength">The length of each hex element in the enumeration</param>
        public IEnumerable<Hex> ToEnumerable(int elementLength)
        {
            var results = new List<Hex>();

            var current = Value;

            while (current.Length > 0)
            {
                var segment = current.Length >= elementLength 
                    ? current.Substring(0, elementLength) 
                    : current;
                
                results.Add(new Hex(segment));

                current = current.Remove(0, segment.Length);
            }

            return results;
        }

        /// <summary>
        /// Reverses the current hex value bytes.
        /// </summary>
        /// <returns>Hex value representing the original hex with all bytes in reverse order</returns>
        /// <remarks>
        /// Reverses by bytes length (2 character). If the length is odd then the last will be the first character of the
        /// original. 
        /// </remarks>
        public Hex Reverse()
        {
            var result = new StringBuilder();

            var current = Value;

            while (current.Length > 0)
            {
                var segment = current.Length >= ByteLength
                    ? current.Substring(current.Length - ByteLength, ByteLength) 
                    : current;
                
                result.Append(segment);

                current = current.Remove(current.Length - segment.Length, segment.Length);
            }

            return new Hex(result.ToString());
        }
        
        /// <summary>
        /// Gets the beginning segment of the current hex value 
        /// </summary>
        /// <param name="length">The length of the segment to get</param>
        /// <returns>Hex value representing the beginning value of the original hex value</returns>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of length is greater that the
        /// hex length or if the value length is less than zero</exception>
        public Hex Head(int length)
        {
            ValidateLength(length);
            
            var result = Value.Substring(0, length);
            
            return new Hex(result);
        }
        
        /// <summary>
        /// Gets the ending segment of the current hex value 
        /// </summary>
        /// <param name="length">The length of the segment to get</param>
        /// <returns>Hex value representing the ending value of the original hex value</returns>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of length is greater that the
        /// hex length or if the value length is less than zero</exception>
        public Hex Tail(int length)
        {
            ValidateLength(length);
            
            var result = Value.Substring(Value.Length - length, length);
            
            return new Hex(result);
        }

        /// <summary>
        /// Removes the beginning segment of the current hex value and returns the resulting segment hex 
        /// </summary>
        /// <param name="length">The length of the segment to drop</param>
        /// <returns>Hex value representing the original value without the specified beginning segment</returns>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of length is greater that the
        /// hex length or if the value length is less than zero</exception>
        public Hex DropHead(int length)
        {
            ValidateLength(length);
            
            var result = Value.Remove(0, length);
            
            return new Hex(result);
        }
        
        /// <summary>
        /// Removes the ending segment of the current hex value and returns the resulting segment hex 
        /// </summary>
        /// <param name="length">The length of the segment to drop</param>
        /// <returns>Hex value representing the original value without the specified ending segment</returns>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of length is greater that the
        /// hex length or if the value length is less than zero</exception>
        public Hex DropTail(int length)
        {
            ValidateLength(length);
                    
            var result = Value.Remove(Value.Length - length, length);
            
            return new Hex(result);
        }

        /// <summary>
        /// Get the beginning segment of the current hex and removes it from the current hex value.
        /// </summary>
        /// <param name="length">The length of the segment to consume</param>
        /// <returns>Hex value representing the consumed segment of the original hex value</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of length is greater that the
        /// hex length or if the value length is less than zero</exception>
        /// <remarks>
        /// Consume will mutate the original hex value by removing the beginning segment from the original hex value.
        /// This is helpful when traversing the string and processing smaller chunks of hex. 
        /// </remarks>
        public Hex Consume(int length)
        {
            ValidateLength(length);
            
            var consumed = Value.Substring(0, length);
            
            Value = Value.Remove(0, length);
            
            return new Hex(consumed);
        }
        
        /// <summary>
        /// Gets a segment of the current hex string value.
        /// </summary>
        /// <param name="start">The index of the segment to start at</param>
        /// <param name="length">The length of the segment to take></param>
        /// <returns>Hex value representing the portion of the hex value taken.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of start + length is greater that the
        /// hex length or if the value of start or length is less than zero</exception>
        /// <remarks>
        /// Take does not mutate the original Hex value. It simply calls SubString and returns the value
        /// wrapped in a hex type.
        /// </remarks>
        public Hex Take(int start, int length)
        {
            if (start + length > Length)
                throw new ArgumentOutOfRangeException(nameof(length),
                    "Start plus length must be less than the length of the hex value");
            
            if (start < 0 || length < 0)
                throw new ArgumentOutOfRangeException(nameof(length),
                    "Start and length must not be less than zero");
            
            var segment = Value.Substring(start, length);
            
            return new Hex(segment);
        }
        
        /// <summary>
        /// Determines is the given Hex is equal to the current hex value
        /// </summary>
        /// <param name="other">The Hex value to compare against</param>
        /// <returns>True if the Hex is equal in value or reference to the current hex</returns>
        public bool Equals(Hex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        /// <summary>
        /// Determines is the given object is equal to the current hex value
        /// </summary>
        /// <param name="obj">The object to compare against</param>
        /// <returns>True if the object is equal in value or reference to the current hex</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Hex)obj);
        }

        /// <summary>
        /// Gets the hash code for the current object value
        /// </summary>
        /// <returns>Value for hash code</returns>
        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        /// <summary>
        /// Equals to implicit operator override
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>True is the values of left an right are equal</returns>
        public static bool operator ==(Hex left, Hex right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not equals to implicit operator override
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>True is the values of left an right are not equal</returns>
        public static bool operator !=(Hex left, Hex right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Determines if the provided string is a hex value
        /// </summary>
        /// <param name="input">The hex string input</param>
        /// <returns>True if the string is empty, the string contains only hex characters [0-9a-fA-F], or if the
        /// string starts with the hex specifier and contains only hex characters</returns>
        private static bool IsValidHex(string input)
        {
            return Regex.IsMatch(input, @"^$") 
                   || Regex.IsMatch(input, @"^[0-9a-fA-F]+$") 
                   || Regex.IsMatch(input, @"^0x[0-9a-fA-F]+$");
        }
        
        /// <summary>
        /// Helper for validating the provided index as a index withing the current value range
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= Value.Length)
                throw new IndexOutOfRangeException("Index out of range");
        }
        
        /// <summary>
        /// Helper for throwing argument out of range exception or a given length
        /// </summary>
        /// <param name="length"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ValidateLength(int length)
        {
            if (length > Length)
                throw new ArgumentOutOfRangeException(nameof(length),
                    "Length must be less than the length of the hex value");
            
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length),
                    "Length must not be less than zero");
        }
    }
}