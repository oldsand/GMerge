using System.Collections.Generic;
using System.Xml.Linq;

namespace GalaxyMerge.IO.Abstractions
{
    public interface ISymbolFile : IGalaxyFile
    {
        XElement Root { get; }
        IEnumerable<XElement> CustomProperties { get; }
        IEnumerable<XElement> WizardOptions { get; }
        XElement VisualTree { get; }
        IEnumerable<XElement> Scripts { get; }
        IEnumerable<string> GetEmbeddedSymbols();
    }
}