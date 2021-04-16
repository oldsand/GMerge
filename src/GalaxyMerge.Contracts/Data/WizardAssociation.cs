using System;
using System.Xml.Linq;
using GalaxyMerge.Common.Abstractions;
using GalaxyMerge.Common.Primitives;

namespace GalaxyMerge.Contracts.Data
{
    public class WizardAssociation : IWizardAssociation
    {
        public string Name { get; set; }
        public WizardAssociationType AssociationType { get; set; }

        public IWizardAssociation FromXml(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;  
            AssociationType = (WizardAssociationType) Enum.Parse(typeof(WizardAssociationType), element.Name.ToString());
            return this;
        }

        public XElement ToXml()
        {
            var element = new XElement(AssociationType.ToString());
            element.Add(new XAttribute(nameof(Name), Name));
            return element;
        }
    }
}