using System.Xml.Linq;
using GalaxyMerge.Common.Abstractions;
using GalaxyMerge.Core;

namespace GalaxyMerge.Archestra.Entities
{
    public class CustomProperty : IXmlConvertible<CustomProperty>
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool Locked { get; set; }
        public string Visibility { get; set; }
        public bool Overridden { get; set; }
        public string Expression { get; set; }
        public string Description { get; set; }

        public CustomProperty FromXml(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value;
            Locked = bool.Parse(element.Attribute(nameof(Locked))?.Value ?? string.Empty);
            Visibility = element.Attribute(nameof(Visibility))?.Value;
            Overridden = bool.Parse(element.Attribute(nameof(Overridden))?.Value ?? string.Empty);
            Expression = element.Element(nameof(Expression))?.FirstAttribute.Value;
            Description = element.Element(nameof(Description))?.Value;
            return this;
        }

        public XElement ToXml()
        {
            var element = new XElement("CustomProperty");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(DataType), DataType));
            element.Add(new XAttribute(nameof(Locked), Locked));
            element.Add(new XAttribute(nameof(Visibility), Visibility));
            element.Add(new XAttribute(nameof(Overridden), Overridden));
            var expression = new XElement(nameof(Expression));
            expression.Add(new XAttribute("Text", Expression));
            element.Add(expression);
            element.Add(new XElement(nameof(Description), Description));
            return element;
        }
    }
}