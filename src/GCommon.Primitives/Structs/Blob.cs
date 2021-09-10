using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Primitives.Structs
{
    public struct Blob : IDifferentiable<Blob>
    {
        public int Guid { get; private set; } 
        public byte[] Data { get; private set; }

        public static Blob Empty => FromData(new byte[] {0});

        public static Blob FromData(byte[] data, int guid = -1)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            return new Blob
            {
                Guid = guid,
                Data = data
            };
        }

        public bool Equals(Blob other)
        {
            return Guid == other.Guid && Data.SequenceEqual(other.Data);
        }

        public IEnumerable<Difference> DiffersFrom(Blob other)
        {
            var differences = new List<Difference>();
            differences.AddRange(Difference.Between(this, other, x => x.Guid));
            differences.AddRange(Difference.Between(this, other, x => x.Data));
            return differences;
        }

        public override bool Equals(object obj)
        {
            return obj is Blob other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Guid * 397) ^ (Data != null ? Data.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Blob left, Blob right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Blob left, Blob right)
        {
            return !left.Equals(right);
        }
    }
}