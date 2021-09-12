using System.Xml.Linq;

namespace GCommon.Primitives.Abstractions
{
    public interface IXSerializable
    {
        XElement Serialize();
    }
}