using System.Runtime.Serialization;
using GCommon.Core;
using GCommon.Core.Enumerations;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class InclusionSettingData
    {
        [DataMember] public Template Template { get; set; }
        [DataMember] public InclusionOption InclusionOption { get; set; }
        [DataMember] public bool IncludeInstances { get; set; }
    }
}