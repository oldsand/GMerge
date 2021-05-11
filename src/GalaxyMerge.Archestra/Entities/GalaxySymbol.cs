using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GalaxyMerge.Common.Abstractions;

namespace GalaxyMerge.Archestra.Entities
{
    public class GalaxySymbol : IGalaxySymbol
    {
        public GalaxySymbol(string tagName)
        {
            TagName = tagName;
        }
        
        public string TagName { get; private set; }
        public XElement Root { get; set; }
        public IEnumerable<ICustomProperty> CustomProperties { get; set; }
        public IEnumerable<IPredefinedScript> PredefinedScripts { get; set; }
        public IEnumerable<INamedScript> NamedScripts { get; set; }
        public XElement VisualTree { get; set; }
        public IEnumerable<IWizardOption> WizardOptions { get; set; }
        public IEnumerable<IWizardLayer> WizardLayers { get; set; }

        public IGalaxySymbol FromXml(XElement element)
        {
            var root = new XElement(element);
            root.RemoveNodes();
            Root = root;
            CustomProperties = element.Descendants("CustomProperty").Select(p => new CustomProperty().FromXml(p)).ToList();
            WizardOptions = element.Element("WizardOptions")?.Elements().Select(o => new WizardOption().FromXml(o)).ToList();
            WizardLayers = element.Descendants("Layer").Select(l => new WizardLayer().FromXml(l)).ToList();
            VisualTree = element.Element("GraphicElements");
            PredefinedScripts = element.Element("PredefinedScripts")?.Descendants().Select(n => new PredefinedScript().FromXml(n)).ToList();;
            NamedScripts = element.Descendants("NamedScript").Select(n => new NamedScript().FromXml(n)).ToList();
            return this;
        }

        public XElement ToXml()
        {
            var root = Root;
            
            if (CustomProperties != null)
                root.Add(new XElement("CustomProperties", CustomProperties.Select(p => p.ToXml())));
            if (WizardOptions != null)
                root.Add(new XElement("WizardOptions", WizardOptions.Select(o => o.ToXml())));
            if (WizardLayers != null)
                root.Add(new XElement("WizardLayers", WizardLayers.Select(l => l.ToXml())));
            if (VisualTree != null)
                root.Add(VisualTree);
            
            var predefined = new XElement("PredefinedScripts");
            if (PredefinedScripts != null)
                predefined.Add(PredefinedScripts.Select(s => s.ToXml()));
            root.Add(predefined);

            var named = new XElement("NamedScripts");
            if (PredefinedScripts != null)
                named.Add(PredefinedScripts.Select(s => s.ToXml()));
            root.Add(named);

            return root;
        }
    }
}