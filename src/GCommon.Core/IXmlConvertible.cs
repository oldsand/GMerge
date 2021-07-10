using System.Xml.Linq;

namespace GCommon.Core
{
    public interface IXmlConvertible<out T>
    {
        XElement ToXml();
        T FromXml(XElement element);
    }
}