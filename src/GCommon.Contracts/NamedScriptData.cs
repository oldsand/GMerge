using System.Runtime.Serialization;
using GCommon.Core;
using GCommon.Core.Enumerations;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class NamedScriptData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int DeadBand { get; set; }
        [DataMember] public string Expression { get; set; }
        [DataMember] public ScriptTrigger Trigger { get; set; }
        [DataMember] public int TriggerPeriod { get; set; }
        [DataMember] public string Text { get; set; }
    }
}