using System.Runtime.Serialization;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class PredefinedScriptData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Text { get; set; }
    }
}