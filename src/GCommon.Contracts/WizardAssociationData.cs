using System.Runtime.Serialization;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class WizardAssociationData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public WizardAssociationType AssociationType { get; set; }
    }
}