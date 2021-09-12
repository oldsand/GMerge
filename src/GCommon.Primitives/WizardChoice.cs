using System.Collections.Generic;
using System.Xml.Linq;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;
using GCommon.Primitives.Abstractions;

namespace GCommon.Primitives
{
    public class WizardChoice : IXSerializable, IDifferentiable<WizardChoice>
    {
        private WizardChoice(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            Rule = element.Attribute(nameof(Rule))?.Value;
        }
        
        public string Name { get; set; }
        public string Rule { get; set; }

        public static WizardChoice Materialize(XElement element)
        {
            return new WizardChoice(element);
        }

        public XElement Serialize()
        {
            var element = new XElement("Choice");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));
            return element;
        }
        
        public bool Equals(WizardChoice other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Rule == other.Rule;
        }

        public IEnumerable<Difference> DiffersFrom(WizardChoice other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.Name));
            differences.AddRange(Difference.Between(this, other, x => x.Rule));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WizardChoice) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Rule != null ? Rule.GetHashCode() : 0);
            }
        }

        public static bool operator ==(WizardChoice left, WizardChoice right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WizardChoice left, WizardChoice right)
        {
            return !Equals(left, right);
        }
    }
}