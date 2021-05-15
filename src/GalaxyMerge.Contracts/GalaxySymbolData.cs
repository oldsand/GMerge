using System.Collections.Generic;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Entities;

namespace GalaxyMerge.Contracts
{
    public class GalaxySymbolData
    {
        public string TagName { get; set; }
        public XElement Root { get; set; }
        public IEnumerable<CustomProperty> CustomProperties { get; set; }
        public IEnumerable<Contracts.PredefinedScriptData> PredefinedScripts { get; set; }
        public IEnumerable<Contracts.NamedScriptData> NamedScripts { get; set; }
        public XElement VisualTree { get; set; }
        public IEnumerable<Contracts.WizardOptionData> WizardOptions { get; set; }
        public IEnumerable<Contracts.WizardLayerData> WizardLayers { get; set; }
    }
}