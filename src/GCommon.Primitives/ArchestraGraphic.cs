using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Primitives.Abstractions;

namespace GCommon.Primitives
{
    public class ArchestraGraphic : IXSerializable
    {
        public ArchestraGraphic(string tagName)
        {
            TagName = tagName;
        }

        private ArchestraGraphic(XElement element)
        {
            var root = new XElement(element);
            root.RemoveNodes();
            Root = root;
            CustomProperties = element.Descendants("CustomProperty").Select(CustomProperty.Materialize).ToList();
            WizardOptions = element.Element("WizardOptions")?.Elements().Select(WizardOption.Materialize).ToList();
            WizardLayers = element.Descendants("Layer").Select(WizardLayer.Materialize).ToList();
            VisualTree = element.Element("GraphicElements");
            PredefinedScripts = element.Element("PredefinedScripts")?.Descendants().Select(n => new PredefinedScript().Materialize(n)).ToList();;
            NamedScripts = element.Descendants("NamedScript").Select(n => new NamedScript().Materialize(n)).ToList();
        }
        
        public string TagName { get; private set; }
        public XElement Root { get; set; }
        public IEnumerable<CustomProperty> CustomProperties { get; set; }
        public IEnumerable<PredefinedScript> PredefinedScripts { get; set; }
        public IEnumerable<NamedScript> NamedScripts { get; set; }
        public XElement VisualTree { get; set; }
        public IEnumerable<WizardOption> WizardOptions { get; set; }
        public IEnumerable<WizardLayer> WizardLayers { get; set; }

        public static ArchestraGraphic Materialize(XElement element)
        {
            return new ArchestraGraphic(element);
        }

        public XElement Serialize()
        {
            var root = Root;
            
            if (CustomProperties != null)
                root.Add(new XElement("CustomProperties", CustomProperties.Select(p => p.Serialize())));
            if (WizardOptions != null)
                root.Add(new XElement("WizardOptions", WizardOptions.Select(o => o.Serialize())));
            if (WizardLayers != null)
                root.Add(new XElement("WizardLayers", WizardLayers.Select(l => l.Serialize())));
            if (VisualTree != null)
                root.Add(VisualTree);
            
            var predefined = new XElement("PredefinedScripts");
            if (PredefinedScripts != null)
                predefined.Add(PredefinedScripts.Select(s => s.Serialize()));
            root.Add(predefined);

            var named = new XElement("NamedScripts");
            if (PredefinedScripts != null)
                named.Add(PredefinedScripts.Select(s => s.Serialize()));
            root.Add(named);

            return root;
        }
    }
}