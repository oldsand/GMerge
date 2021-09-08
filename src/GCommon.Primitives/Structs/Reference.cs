using System;
using System.Collections.Generic;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Primitives.Structs
{
    public struct Reference : IDifferentiable<Reference>
    {
        public Reference(string reference, string objectName = null) 
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            var dotIndex = reference.IndexOf(".");
            
            objectName ??= dotIndex > 0 
                ? reference.Substring(0, dotIndex) 
                : string.Empty;
            
            var attributeName = dotIndex > 0
                ? reference.Substring(dotIndex + 1, reference.Length - (dotIndex + 1))
                : string.Empty;

            FullName = reference;
            ObjectName = objectName;
            AttributeName = attributeName;
        }
        
        public const string DefaultReference = "---";
        public string FullName { get; private set; }
        public string ObjectName { get; set; }
        public string AttributeName { get; set; }
        public static Reference Empty => new Reference(DefaultReference);
        public static Reference Auto => new Reference("---Auto---");

        public override string ToString()
        {
            return FullName;
        }

        public bool Equals(Reference other)
        {
            return FullName == other.FullName && ObjectName == other.ObjectName && AttributeName == other.AttributeName;
        }

        public IEnumerable<Difference> DiffersFrom(Reference other)
        {
            var differences = new List<Difference>();
            differences.AddRange(Difference.Between(this, other, x => x.FullName));
            differences.AddRange(Difference.Between(this, other, x => x.ObjectName));
            differences.AddRange(Difference.Between(this, other, x => x.AttributeName));
            return differences;
        }

        public override bool Equals(object obj)
        {
            return obj is Reference other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ObjectName != null ? ObjectName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AttributeName != null ? AttributeName.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Reference left, Reference right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Reference left, Reference right)
        {
            return !left.Equals(right);
        }
    }
}