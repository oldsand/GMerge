using System.Collections.Generic;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IGalaxyObject : IXmlConvertible<IGalaxyObject>
    {
        string TagName { get; }
        string HierarchicalName { get; }
        string ContainedName { get; }
        int ConfigVersion { get; }
        string DerivedFromName { get; }
        string BasedOnName { get; }
        ObjectCategory Category { get; }
        string HostName { get; }
        string AreaName { get; }
        string ContainerName { get; }
        IEnumerable<IGalaxyAttribute> Attributes { get; }
    }
}