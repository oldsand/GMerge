using System.Xml.Linq;

namespace GCommon.Primitives.Base
{
    public interface IXSerializable
    {
        XElement Serialize();
    }
}