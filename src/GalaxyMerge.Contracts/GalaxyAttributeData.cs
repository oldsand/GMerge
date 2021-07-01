using System.Runtime.Serialization;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class GalaxyAttributeData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public DataType DataType { get; set; }
        [DataMember] public AttributeCategory Category { get; set; }
        [DataMember] public SecurityClassification Security { get; set; }
        [DataMember] public LockType Locked { get; set; }
        [DataMember] public object Value { get; set; }
        [DataMember] public int ArrayCount { get; set; }
    }
}