using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class WizardLayerData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Rule { get; set; }
        [DataMember] public IEnumerable<WizardAssociationData> Associations { get; set; }
    }
}