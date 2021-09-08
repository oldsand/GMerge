using System.Collections.Generic;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Primitives.Structs
{
    public readonly struct Enumeration : IDifferentiable<Enumeration>
    {
        public Enumeration(string value, short ordinal = 0, short primitiveId = 0, short attributeId = 0)
        {
            Value = value;
            Ordinal = ordinal;
            PrimitiveId = primitiveId;
            AttributeId = attributeId;
        }
        
        public string Value { get; }
        public short Ordinal { get; }
        public short PrimitiveId { get; }
        public short AttributeId { get; }

        public bool Equals(Enumeration other)
        {
            return Value == other.Value && Ordinal == other.Ordinal && PrimitiveId == other.PrimitiveId && AttributeId == other.AttributeId;
        }

        public IEnumerable<Difference> DiffersFrom(Enumeration other)
        {
            var differences = new List<Difference>();
            differences.AddRange(Difference.Between(this, other, x => x.Value));
            differences.AddRange(Difference.Between(this, other, x => x.Ordinal));
            differences.AddRange(Difference.Between(this, other, x => x.PrimitiveId));
            differences.AddRange(Difference.Between(this, other, x => x.AttributeId));
            return differences;
        }

        public override bool Equals(object obj)
        {
            return obj is Enumeration other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value != null ? Value.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Ordinal.GetHashCode();
                hashCode = (hashCode * 397) ^ PrimitiveId.GetHashCode();
                hashCode = (hashCode * 397) ^ AttributeId.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !left.Equals(right);
        }
    }
}