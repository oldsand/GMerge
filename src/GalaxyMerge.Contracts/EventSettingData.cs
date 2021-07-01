using System.Runtime.Serialization;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class EventSettingData
    {
        [DataMember] public Operation Operation { get; set; }
        [DataMember] public OperationType OperationType { get; set; }
        [DataMember] public bool IsArchiveTrigger { get; set; }
    }
}