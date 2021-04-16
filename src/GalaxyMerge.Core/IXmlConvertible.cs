using System.Xml.Linq;

namespace GalaxyMerge.Core
{
    public interface IXmlConvertible<out T>
    {
        T FromXml(XElement element);
        XElement ToXml();
    }
}