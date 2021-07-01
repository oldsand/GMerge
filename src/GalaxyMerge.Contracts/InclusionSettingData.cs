using System.Runtime.Serialization;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class InclusionSettingData
    {
        [DataMember] public Template Template { get; set; }
        [DataMember] public InclusionOption InclusionOption { get; set; }
        [DataMember] public bool IncludeInstances { get; set; }
    }
}