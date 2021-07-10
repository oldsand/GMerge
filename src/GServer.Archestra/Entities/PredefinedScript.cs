using System.Xml.Linq;
using GCommon.Core;

namespace GServer.Archestra.Entities
{
    public class PredefinedScript : IXmlConvertible<PredefinedScript>
    {
        public string Name { get; set; }
        public string Text { get; set; }
        
        public PredefinedScript FromXml(XElement element)
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