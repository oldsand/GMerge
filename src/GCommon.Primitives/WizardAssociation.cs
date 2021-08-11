using System;
using System.Xml.Linq;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class WizardAssociation : IXSerializable
    {
        public string Name { get; set; }
        public WizardAssociationType AssociationType { get; set; }

        public WizardAssociation Materialize(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;  
            AssociationType = (WizardAssociationType) Enum.Parse(typeof(WizardAssociationType), element.Name.ToString());
            return this;
        }

        public XElement Serialize()
        {
            var element = new XElement(AssociationType.ToString());
            element.Add(new XAttribute(nameof(Name), Name));
            return element;
        }
    }
}