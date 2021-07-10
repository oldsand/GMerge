using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class GalaxySymbolData
    {
        [DataMember] public string TagName { get; set; }
        [DataMember] public XElement Root { get; set; }
        [DataMember] public IEnumerable<CustomPropertyData> CustomProperties { get; set; }
        [DataMember] public IEnumerable<PredefinedScriptData> PredefinedScripts { get; set; }
        [DataMember] public IEnumerable<NamedScriptData> NamedScripts { get; set; }
        [DataMember] public XElement VisualTree { get; set; }
        [DataMember] public IEnumerable<WizardOptionData> WizardOptions { get; set; }
        [DataMember] public IEnumerable<WizardLayerData> WizardLayers { get; set; }
    }
}