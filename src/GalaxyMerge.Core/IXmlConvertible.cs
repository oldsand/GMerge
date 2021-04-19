using System.Xml.Linq;

namespace GalaxyMerge.Core
{
    public interface IXmlConvertible<out T>
    {
        XElement ToXml();
        T FromXml(XElement element);
    }
}