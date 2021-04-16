using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GalaxyMerge.Core.Extensions
{
    public static class XmlSerializationExtension
    {
        public static string Serialize<T>(this T obj) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }
        
        public static string Serialize<T>(this T obj, string root) where T : class
        {
            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));
            using var writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        public static T Deserialize<T>(this string xml) where T : class
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            var serializer = new XmlSerializer(typeof(T));

            using var reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader);
        }
    }
}