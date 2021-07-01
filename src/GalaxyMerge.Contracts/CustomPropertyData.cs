using System.Runtime.Serialization;

namespace GalaxyMerge.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class CustomPropertyData
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string DataType { get; set; }
        [DataMember] public bool Locked { get; set; }
        [DataMember] public string Visibility { get; set; }
        [DataMember] public bool Overridden { get; set; }
        [DataMember] public string Expression { get; set; }
        [DataMember] public string Description { get; set; }
    }
}