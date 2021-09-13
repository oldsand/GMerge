using System.Collections.Generic;
using System.Runtime.Serialization;
using GCommon.Core;
using GCommon.Core.Enumerations;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class WizardOptionData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public WizardOptionType OptionType { get; set; }
        [DataMember] public string Rule { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string DefaultValue { get; set; }
        [DataMember] public IEnumerable<WizardChoiceData> Choices { get; set; }
    }
}