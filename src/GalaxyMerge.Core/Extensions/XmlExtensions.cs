using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GalaxyMerge.Core.Extensions
{
    public static class XmlExtensions
    {
        public static string SimplePath(this XElement element)
        {
            var tmp = element;
            var path = string.Empty;
            while (tmp != null)
            {
                path = "/" + tmp.Name + path;
                tmp = tmp.Parent;
            }
            return path;
        }

        public static XElement Path(this XElement element)
        {
            if (element.Parent == null) return null;
            
            var current = element.Parent;
            current.RemoveNodes();
            current.NodesBeforeSelf().Remove();
            current.NodesAfterSelf().Remove();
            
            while (current?.Parent != null)
            {
                current.NodesBeforeSelf().Remove();
                current.NodesAfterSelf().Remove();
                current = current.Parent;
            }

            return current;
        }
        
        public static XElement PathAndSelf(this XElement element)
        {
            var current = element;
            while (current?.Parent != null)
            {
                current.NodesBeforeSelf().Remove();
                current.NodesAfterSelf().Remove();
                current = current.Parent;
            }

            return current;
        }

        public static XElement PathAndShallowSelf(this XElement element)
        {
            element.RemoveNodes();
            var current = element;
            while (current?.Parent != null)
            {
                current.NodesBeforeSelf().Remove();
                current.NodesAfterSelf().Remove();
                current = current.Parent;
            }

            return current;
        }

        public static byte [] ToByteArray(this XNode node, SaveOptions options = default, Encoding encoding = default)
        {
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = (options & SaveOptions.DisableFormatting) == 0, Encoding = encoding ?? Encoding.Default };
            if ((options & SaveOptions.OmitDuplicateNamespaces) != 0)
                settings.NamespaceHandling |= NamespaceHandling.OmitDuplicates;
            return node.ToByteArray(settings);
        }
    
        public static byte [] ToByteArray(this XNode node, XmlWriterSettings settings)
        {
            using var memoryStream = new MemoryStream();
            using (var writer = XmlWriter.Create(memoryStream, settings)) 
                node.WriteTo(writer);
            return memoryStream.ToArray();
        }
    }
}