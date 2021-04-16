using System.Xml.Linq;
using GalaxyMerge.Common.Abstractions;

namespace GalaxyMerge.Contracts.Data
{
    public class WizardChoice : IWizardChoice
    {
        public string Name { get; set; }
        public string Rule { get; set; }

        public IWizardChoice FromXml(XElement element)
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