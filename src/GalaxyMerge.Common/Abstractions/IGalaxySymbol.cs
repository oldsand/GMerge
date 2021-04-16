using System.Collections.Generic;
using System.Xml.Linq;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IGalaxySymbol : IXmlConvertible<IGalaxySymbol>
    {
        string TagName { get; }
        XElement Root { get; }
        IEnumerable<ICustomProperty> CustomProperties { get; }
        IEnumerable<IPredefinedScript> PredefinedScripts { get; }
        IEnumerable<INamedScript> NamedScripts { get; }
        XElement VisualTree { get; }
        IEnumerable<IWizardOption> WizardOptions { get; }
        IEnumerable<IWizardLayer> WizardLayers { get; }
    }
}