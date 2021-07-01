using System.Collections.Generic;
using System.Runtime.Serialization;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
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