using System.Runtime.Serialization;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class WizardChoiceData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Rule { get; set; }
    }
}