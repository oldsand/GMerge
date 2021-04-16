using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IPredefinedScript : IXmlConvertible<IPredefinedScript>
    {
        string Name { get; }
        string Text { get; }
    }
}