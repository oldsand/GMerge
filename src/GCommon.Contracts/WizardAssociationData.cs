using System.Runtime.Serialization;
using GCommon.Primitives;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class WizardAssociationData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public WizardAssociationType AssociationType { get; set; }
    }
}