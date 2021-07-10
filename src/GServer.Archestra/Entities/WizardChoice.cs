using System.Xml.Linq;
using GCommon.Core;

namespace GServer.Archestra.Entities
{
    public class WizardChoice : IXmlConvertible<WizardChoice>
    {
        public string Name { get; set; }
        public string Rule { get; set; }

        public WizardChoice FromXml(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            Rule = element.Attribute(nameof(Rule))?.Value;
            return this;
        }

        public XElement ToXml()
        {
            var element = new XElement("Choice");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));
            return element;
        }
    }
}