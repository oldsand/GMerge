using System;
using System.Collections.Generic;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Primitives.Structs
{
    public struct Reference : IDifferentiable<Reference>
    {
        public const string DefaultReference = "---";
        public string FullName { get; private set; }
        public string ObjectName { get; set; }
        public string AttributeName { get; set; }
        
        public override string ToString()
        {
            return FullName;
        }
        
        public static Reference Empty()
        {
            return Reference.FromName(DefaultReference);
        }

        public static Reference FromName(string fullName)
        {
            if (fullName == null)
                throw new ArgumentNullException(nameof(fullName));

            var dotIndex = fullName.IndexOf(".");
            
            var objectName = dotIndex > 0 
                ? fullName.Substring(0, dotIndex) 
                : string.Empty;
            
            var attributeName = dotIndex > 0
                ? fullName.Substring(dotIndex + 1, fullName.Length - (dotIndex + 1))
                : string.Empty;
            
            return new Reference()
            {
                FullName = fullName,
                ObjectName = objectName,
                AttributeName = attributeName
            };
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