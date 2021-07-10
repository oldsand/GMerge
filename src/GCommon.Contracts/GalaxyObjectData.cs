using System.Collections.Generic;
using System.Runtime.Serialization;
using GCommon.Primitives;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class GalaxyObjectData
    {
        [DataMember] public string TagName { get; set; }
        [DataMember] public string ContainedName { get; set; }
        [DataMember] public string HierarchicalName { get; set; }
        [DataMember] public int ConfigVersion { get; set; }
        [DataMember] public string DerivedFromName { get; set; }
        [DataMember] public string BasedOnName { get; set; }
        [DataMember] public ObjectCategory Category { get; set; }
        [DataMember] public string HostName { get; set; }
        [DataMember] public string AreaName { get; set; }
        [DataMember] public string ContainerName { get; set; }
        [DataMember] public IEnumerable<GalaxyAttributeData> Attributes { get; set; }
    }
}