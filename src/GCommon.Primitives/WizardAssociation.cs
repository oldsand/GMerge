using System;
using System.Collections.Generic;
using System.Xml.Linq;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class WizardAssociation : IXSerializable, IDifferentiable<WizardAssociation>
    {
        private WizardAssociation(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            AssociationType = WizardAssociationType.FromName(element.Name.ToString());
        }

        public string Name { get; set; }
        public WizardAssociationType AssociationType { get; set; }

        public static WizardAssociation Materialize(XElement element)
        {
            return new WizardAssociation(element);
        }

        public XElement Serialize()
        {
            var element = new XElement(AssociationType.Name);
            element.Add(new XAttribute(nameof(Name), Name));
            return element;
        }

        public bool Equals(WizardAssociation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Equals(AssociationType, other.AssociationType);
        }

        public IEnumerable<Difference> DiffersFrom(WizardAssociation other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.Name));
            differences.AddRange(Difference.Between(this, other, x => x.AssociationType));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WizardAssociation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^
                       (AssociationType != null ? AssociationType.GetHashCode() : 0);
            }
        }

        public static bool operator ==(WizardAssociation left, WizardAssociation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WizardAssociation left, WizardAssociation right)
        {
            return !Equals(left, right);
        }
    }
}