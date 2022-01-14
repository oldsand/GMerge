using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Core.Abstractions;
using GCommon.Core.Enumerations;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Core
{
    public class WizardLayer : IXSerializable, IDifferentiable<WizardLayer>
    {
       public WizardLayer(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            Rule = element.Attribute(nameof(Rule))?.Value;
            Associations = element.Descendants().Where(d => d.HasAttributes)
                .Select(WizardAssociation.Materialize).ToList();
        }
       
        public string Name { get; set; }
        public string Rule { get; set; }
        public IEnumerable<WizardAssociation> Associations { get; set; }

        public static WizardLayer Materialize(XElement element)
        {
            return new WizardLayer(element);
        }

        public XElement Serialize()
        {
            var element = new XElement("Layer");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));

            if (Associations.Any(a => a.AssociationType == WizardAssociationType.Element))
            {
                var graphicElements = new XElement("GraphicElements");
                graphicElements.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.Element)
                    .Select(a => a.Serialize()));
                element.Add(graphicElements);
            }
            
            if (Associations.Any(a => a.AssociationType == WizardAssociationType.Script))
            {
                var namedScripts = new XElement("NamedScripts");
                namedScripts.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.Script)
                    .Select(a => a.Serialize()));
                element.Add(namedScripts);
            }
            
            if (Associations.Any(a => a.AssociationType == WizardAssociationType.CustomProperty))
            {
                var customProperties = new XElement("CustomProperties");
                customProperties.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.CustomProperty)
                    .Select(a => a.Serialize()));
                element.Add(customProperties);
            }

            return element;
        }
        
        public bool Equals(WizardLayer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Rule == other.Rule && Equals(Associations, other.Associations);
        }

        public IEnumerable<Difference> DiffersFrom(WizardLayer other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.Name));
            differences.AddRange(Difference.Between(this, other, x => x.Rule));
            differences.AddRange(Difference.BetweenCollection(Associations, other.Associations, x => x.Name));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WizardLayer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Rule != null ? Rule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Associations != null ? Associations.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(WizardLayer left, WizardLayer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WizardLayer left, WizardLayer right)
        {
            return !Equals(left, right);
        }
    }
}