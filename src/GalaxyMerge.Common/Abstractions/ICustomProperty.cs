using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface ICustomProperty : IXmlConvertible<ICustomProperty>
    {
        string Name { get; }
        string DataType { get; }
        bool Locked { get; }
        string Visibility { get; }
        bool Overridden { get; }
        string Expression { get; }
        string Description { get; }
    }
}