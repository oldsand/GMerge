using System.Collections.Generic;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class GalaxyObjectData
    {
        public string TagName { get; set; }
        public string ContainedName { get; set; }
        public string HierarchicalName { get; set; }
        public int ConfigVersion { get; set; }
        public string DerivedFromName { get; set; }
        public string BasedOnName { get; set; }
        public ObjectCategory Category { get; set; }
        public string HostName { get; set; }
        public string AreaName { get; set; }
        public string ContainerName { get; set; }
        public IEnumerable<GalaxyAttributeData> Attributes { get; set; }
    }
}