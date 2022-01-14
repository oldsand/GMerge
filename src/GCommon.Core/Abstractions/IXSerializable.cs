using System.Xml.Linq;

namespace GCommon.Core.Abstractions
{
    public interface IXSerializable
    {
        XElement Serialize();
    }
}