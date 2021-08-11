using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class WizardLayer : IXSerializable
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public IEnumerable<WizardAssociation> Associations { get; set; }

        public WizardLayer Materialize(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            Rule = element.Attribute(nameof(Rule))?.Value;
            Associations = element.Descendants().Where(d => d.HasAttributes)
                .Select(a => new WizardAssociation().Materialize(a)).ToList();
            return this;
        }

        public XElement Serialize()
        {
            var element = new XElement("Layer");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));

            if (Associations.Any(a => a.AssociationType == WizardAssociationType.ElementAssociation))
            {
                var graphicElements = new XElement("GraphicElements");
                graphicElements.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.ElementAssociation)
                    .Select(a => a.Serialize()));
                element.Add(graphicElements);
            }
            
            if (Associations.Any(a => a.AssociationType == WizardAssociationType.ScriptAssociation))
            {
                var namedScripts = new XElement("NamedScripts");
                namedScripts.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.ScriptAssociation)
                    .Select(a => a.Serialize()));
                element.Add(namedScripts);
            }
            
            if (Associations.Any(a => a.AssociationType == WizardAssociationType.CustomPropertyAssociation))
            {
                var customProperties = new XElement("CustomProperties");
                customProperties.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.CustomPropertyAssociation)
                    .Select(a => a.Serialize()));
                element.Add(customProperties);
            }

            return element;
        }
    }
}