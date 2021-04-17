using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using GalaxyMerge.Common.Abstractions;
using GalaxyMerge.Common.Primitives;

namespace GalaxyMerge.Contracts.Data
{
    [XmlRoot]
    public class GalaxyObject : IGalaxyObject
    {
        public string TagName { get; set; }
        public string ContainedName { get; set; }
        public string HierarchicalName { get; set; }
        public int ConfigVersion { get; set; }
        public string DerivedFromName { get; set; }
        public string BasedOnName { get; set; }
        public ObjectCategory Category { get; set; }
        public string HostName { get; set; }
        public string AreaName { get; set; }
        public string ContainerName { get; set; }
        public IEnumerable<IGalaxyAttribute> Attributes { get; set; }

        public IGalaxyObject FromXml(XElement element)
        {
            TagName = element.Attribute(nameof(TagName))?.Value;
            HierarchicalName = element.Attribute(nameof(HierarchicalName))?.Value;
            ContainedName = element.Attribute(nameof(ContainedName))?.Value;
            ConfigVersion =  Convert.ToInt32(element.Attribute(nameof(ConfigVersion))?.Value);
            Category = (ObjectCategory) Enum.Parse(typeof(ObjectCategory), element.Attribute(nameof(Category))?.Value ?? string.Empty);
            DerivedFromName =  element.Attribute(nameof(DerivedFromName))?.Value;
            BasedOnName =  element.Attribute(nameof(BasedOnName))?.Value;
            HostName = element.Attribute(nameof(HostName))?.Value;
            AreaName = element.Attribute(nameof(AreaName))?.Value;
            ContainerName = element.Attribute(nameof(ContainerName))?.Value;
            Attributes = element.Descendants("Attribute").Select(a => new GalaxyAttribute().FromXml(a)).ToList();
            return this;
        }

        public XElement ToXml()
        {
            var root = new XElement("Object");
            root.Add(new XAttribute(nameof(TagName), TagName ?? string.Empty));
            root.Add(new XAttribute(nameof(HierarchicalName), HierarchicalName ?? string.Empty));
            root.Add(new XAttribute(nameof(ContainedName), ContainedName ?? string.Empty));
            root.Add(new XAttribute(nameof(ConfigVersion), ConfigVersion));
            root.Add(new XAttribute(nameof(Category), Category));
            root.Add(new XAttribute(nameof(DerivedFromName), DerivedFromName ?? string.Empty));
            root.Add(new XAttribute(nameof(BasedOnName), BasedOnName ?? string.Empty));
            root.Add(new XAttribute(nameof(HostName), HostName ?? string.Empty));
            root.Add(new XAttribute(nameof(AreaName), AreaName ?? string.Empty));
            root.Add(new XAttribute(nameof(ContainerName), ContainerName ?? string.Empty));

            if (Attributes != null)
                root.Add(new XElement("Attributes", Attributes.Select(a => a.ToXml())));

            return root;
        }
    }
}