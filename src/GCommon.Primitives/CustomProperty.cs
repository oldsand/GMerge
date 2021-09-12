using System.Collections.Generic;
using System.Xml.Linq;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;
using GCommon.Primitives.Abstractions;

namespace GCommon.Primitives
{
    public class CustomProperty : IXSerializable, IDifferentiable<CustomProperty>
    {
        public CustomProperty(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value;
            Locked = bool.Parse(element.Attribute(nameof(Locked))?.Value ?? string.Empty);
            Visibility = element.Attribute(nameof(Visibility))?.Value;
            Overridden = bool.Parse(element.Attribute(nameof(Overridden))?.Value ?? string.Empty);
            Expression = element.Element(nameof(Expression))?.FirstAttribute.Value;
            Description = element.Element(nameof(Description))?.Value;
        }

        public string Name { get; set; }
        public string DataType { get; set; }
        public bool Locked { get; set; }
        public string Visibility { get; set; }
        public bool Overridden { get; set; }
        public string Expression { get; set; }
        public string Description { get; set; }

        public static CustomProperty Materialize(XElement element)
        {
            return new CustomProperty(element);
        }

        public XElement Serialize()
        {
            var element = new XElement("CustomProperty");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(DataType), DataType));
            element.Add(new XAttribute(nameof(Locked), Locked));
            element.Add(new XAttribute(nameof(Visibility), Visibility));
            element.Add(new XAttribute(nameof(Overridden), Overridden));
            var expression = new XElement(nameof(Expression));
            expression.Add(new XAttribute("Text", Expression));
            element.Add(expression);
            element.Add(new XElement(nameof(Description), Description));
            return element;
        }

        public bool Equals(CustomProperty other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && DataType == other.DataType && Locked == other.Locked &&
                   Visibility == other.Visibility && Overridden == other.Overridden && Expression == other.Expression &&
                   Description == other.Description;
        }

        public IEnumerable<Difference> DiffersFrom(CustomProperty other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.Name));
            differences.AddRange(Difference.Between(this, other, x => x.DataType));
            differences.AddRange(Difference.Between(this, other, x => x.Locked));
            differences.AddRange(Difference.Between(this, other, x => x.Visibility));
            differences.AddRange(Difference.Between(this, other, x => x.Overridden));
            differences.AddRange(Difference.Between(this, other, x => x.Expression));
            differences.AddRange(Difference.Between(this, other, x => x.Description));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CustomProperty) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DataType != null ? DataType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Locked.GetHashCode();
                hashCode = (hashCode * 397) ^ (Visibility != null ? Visibility.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Overridden.GetHashCode();
                hashCode = (hashCode * 397) ^ (Expression != null ? Expression.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(CustomProperty left, CustomProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CustomProperty left, CustomProperty right)
        {
            return !Equals(left, right);
        }
    }
}