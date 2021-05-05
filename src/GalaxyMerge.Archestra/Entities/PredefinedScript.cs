using System.Xml.Linq;
using GalaxyMerge.Common.Abstractions;

namespace GalaxyMerge.Archestra.Entities
{
    public class PredefinedScript : IPredefinedScript
    {
        public string Name { get; set; }
        public string Text { get; set; }
        
        public IPredefinedScript FromXml(XElement element)
        {
            Name = element.Name.ToString();
            Text = element.Value;
            return this;
        }

        public XElement ToXml()
        {
            return new XElement(Name, Text);
        }
    }
}