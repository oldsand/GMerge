using System.Runtime.Serialization;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class WizardAssociationData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public WizardAssociationType AssociationType { get; set; }
    }
}