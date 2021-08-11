using System.Xml.Linq;
using GCommon.Primitives.Base;

namespace GCommon.Primitives
{
    public class WizardChoice : IXSerializable
    {
        public string Name { get; set; }
        public string Rule { get; set; }

        public WizardChoice Materialize(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            Rule = element.Attribute(nameof(Rule))?.Value;
            return this;
        }

        public XElement Serialize()
        {
            var element = new XElement("Choice");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));
            return element;
        }
    }
}