using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Core;
using GCommon.Primitives;

namespace GServer.Archestra.Entities
{
    public class WizardLayer : IXmlConvertible<WizardLayer>
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public IEnumerable<WizardAssociation> Associations { get; set; }

        public WizardLayer FromXml(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            Rule = element.Attribute(nameof(Rule))?.Value;
            Associations = element.Descendants().Where(d => d.HasAttributes)
                .Select(a => new WizardAssociation().FromXml(a)).ToList();
            return this;
        }

        public XElement ToXml()
        {
            var element = new XElement("Layer");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));

            if (Associations.Any(a => a.AssociationType == WizardAssociationType.ElementAssociation))
            {
                var graphicElements = new XElement("GraphicElements");
                graphicElements.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.ElementAssociation)
                    .Select(a => a.ToXml()));
                element.Add(graphicElements);
            }
            
            if (Associations.Any(a => a.AssociationType == WizardAssociationType.ScriptAssociation))
            {
                var namedScripts = new XElement("NamedScripts");
                namedScripts.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.ScriptAssociation)
                    .Select(a => a.ToXml()));
                element.Add(namedScripts);
            }
            
            if (Associations.Any(a => a.AssociationType == WizardAssociationType.CustomPropertyAssociation))
            {
                var customProperties = new XElement("CustomProperties");
                customProperties.Add(Associations
                    .Where(a => a.AssociationType == WizardAssociationType.CustomPropertyAssociation)
                    .Select(a => a.ToXml()));
                element.Add(customProperties);
            }

            return element;
        }
    }
}