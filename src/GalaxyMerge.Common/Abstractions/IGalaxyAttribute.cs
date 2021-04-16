using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IGalaxyAttribute : IXmlConvertible<IGalaxyAttribute>
    {
        string Name { get; }
        DataType DataType { get; }
        AttributeCategory Category { get; }
        SecurityClassification Security { get; }
        LockType Locked { get; }
        object Value { get; }
        int ArrayCount { get; }
    }
}