using System.Collections.Generic;
using System.Xml.Linq;

namespace GalaxyMerge.Contracts
{
    public class GalaxySymbolData
    {
        public string TagName { get; set; }
        public XElement Root { get; set; }
        public IEnumerable<CustomPropertyData> CustomProperties { get; set; }
        public IEnumerable<PredefinedScriptData> PredefinedScripts { get; set; }
        public IEnumerable<NamedScriptData> NamedScripts { get; set; }
        public XElement VisualTree { get; set; }
        public IEnumerable<WizardOptionData> WizardOptions { get; set; }
        public IEnumerable<WizardLayerData> WizardLayers { get; set; }
    }
}