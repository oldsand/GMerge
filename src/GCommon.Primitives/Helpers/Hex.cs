using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GCommon.Primitives.Helpers
{
    public class Hex : IEquatable<Hex>
    {
        private const int ByteLength = 2;
        
        public Hex(string value)
        {
            if (!IsValidHex(value))
                throw new ArgumentException($"Value '{value}' is not valid hex");
            
            Value = value;
        }
        
        public string Value { get; set; }
        
        public static Hex Empty => new Hex("00");

        public static implicit operator Hex(string value)
        {
            return new Hex(value);
        } 

        public static implicit operator string(Hex hex)
        {
            return hex.Value;
        }

        public Hex this[int index]
        {
            get
            {
                ValidateIndex(index);

                return new Hex(Value[index].ToString());
            }
        }

        public static bool IsValid(string input)
        {
            return IsValidHex(input);
        }

        public bool ToBool()
        {
            return int.Parse(Value, NumberStyles.HexNumber) > 0;
        }

        public int ToInt()
        {
            return int.Parse(Value, NumberStyles.HexNumber);
        }

        public float ToFloat()
        {
            var value = int.Parse(Value, NumberStyles.HexNumber);
            var bytes = BitConverter.GetBytes(value);
            return BitConverter.ToSingle(bytes, 0);
        }

        public double ToDouble()
        {
            var value = long.Parse(Value, NumberStyles.HexNumber);
            var bytes = BitConverter.GetBytes(value);
            return BitConverter.ToDouble(bytes, 0);
        }

        public string ToChar()
        {
            var value = int.Parse(Value, NumberStyles.HexNumber);
            return char.ConvertFromUtf32(value);
        }

        public DateTime ToDateTime()
        {
            var value = long.Parse(Value, NumberStyles.HexNumber);
            return DateTime.FromBinary(value).AddYears(1600).ToLocalTime();
        }

        public TimeSpan ToTimeSpan()
        {
            var value = long.Parse(Value, NumberStyles.HexNumber);
            return TimeSpan.FromTicks(value);
        }
        
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
        
        public Hex Head(int length)
        {
            if (length > Value.Length || length == 0)
                throw new ArgumentException("Length must be greater than zero and less than the length of the hex value");
            
            var result = Value.Substring(0, length);
            
            return new Hex(result);
        }
        
        public Hex Tail(int length)
        {
            if (length > Value.Length || length == 0)
                throw new ArgumentException("Length must be greater than zero and less than the length of the hex value");
            
            var result = Value.Substring(Value.Length - length, length);
            
            return new Hex(result);
        }

        public Hex DropHead(int length)
        {
            if (length >= Value.Length)
                throw new ArgumentException("Length must be less than the length of the hex value");
            
            var result = Value.Remove(0, length);
            
            return new Hex(result);
        }
        
        public Hex DropTail(int length)
        {
            if (length >= Value.Length)
                throw new ArgumentException("Length must be less than the length of the hex value");
                    
            var result = Value.Remove(Value.Length - length, length);
            
            return new Hex(result);
        }
        
        public Hex Take(int start, int length)
        {
            var stripped = Value.Substring(start, length);
            return new Hex(stripped);
        }
        
        public bool Equals(Hex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Hex)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public static bool operator ==(Hex left, Hex right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Hex left, Hex right)
        {
            return !Equals(left, right);
        }

        private static bool IsValidHex(string input)
        {
            return Regex.IsMatch(input, @"^[0-9a-fA-F]+$") || Regex.IsMatch(input, @"^0x[0-9a-fA-F]+$");
        }
        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= Value.Length)
                throw new IndexOutOfRangeException("Index out of range");
        }
    }
}