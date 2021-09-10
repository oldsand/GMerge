using System.Xml;
using System.Xml.Linq;
using ArchestrA.Visualization.GraphicLibrary;
using GCommon.Core.Extensions;

namespace GServer.Archestra.Extensions
{
    internal static class SymbolExtensions
    {
        public static XDocument GenerateXml(this aaSymbolGraphicContainer container)
        {
            var doc = new XmlDocument();
            XmlNode xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(xmlDeclaration);
            var node = doc.CreateNode(XmlNodeType.Element, "aa", "Symbol", "http://www.wonderware.com/ArchestrA");
            var attribute = doc.CreateAttribute("GraphicVersion");
            attribute.Value = aaGraphicVersion.LatestVersion.ToString();
            node.Attributes?.SetNamedItem(attribute);
            doc.AppendChild(node);
            
            container.Save(doc, node, aaXMLFormat.All);

            return doc.ToXDoc();
        }
    }
}