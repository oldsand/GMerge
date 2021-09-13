using System;
using System.Collections.Generic;
using System.Xml.Linq;
using GCommon.Core.Enumerations;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Core.Abstractions
{
    public abstract class ObjectBase : IXSerializable, IDifferentiable<ObjectBase>
    {
        protected ObjectBase(string tagName, Template template, string derivedFrom = null, int version = 1)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            SetTagName(tagName);
            SetContainedName(string.Empty);
            SetHierarchicalName(tagName);
            ConfigVersion = version;
            DerivedFromName = derivedFrom ?? Template.Name;
            HostName = string.Empty;
            AreaName = string.Empty;
            ContainerName = string.Empty;
        }

        protected ObjectBase(string tagName, string hierarchicalName, string containedName, int configVersion,
            Template template, string derivedFromName, string hostName, string areaName, string containerName)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));

            SetTagName(tagName);
            SetContainedName(containedName);
            SetHierarchicalName(hierarchicalName);
            ConfigVersion = configVersion;
            DerivedFromName = derivedFromName;
            HostName = hostName ?? string.Empty;
            AreaName = areaName ?? string.Empty;
            ContainerName = containerName ?? string.Empty;
        }

        protected ObjectBase(XElement element)
        {
            if (element.Name != nameof(ObjectBase))
                throw new InvalidOperationException();

            TagName = element.Attribute(nameof(TagName))?.Value;
            HierarchicalName = element.Attribute(nameof(HierarchicalName))?.Value;
            ContainedName = element.Attribute(nameof(ContainedName))?.Value;
            ConfigVersion = Convert.ToInt32(element.Attribute(nameof(ConfigVersion))?.Value);
            Template = Template.FromName(element.Attribute(nameof(Template))?.Value);
            DerivedFromName = element.Attribute(nameof(DerivedFromName))?.Value;
            HostName = element.Attribute(nameof(HostName))?.Value;
            AreaName = element.Attribute(nameof(AreaName))?.Value;
            ContainerName = element.Attribute(nameof(ContainerName))?.Value;
        }

        public string TagName { get; private set; }
        public string HierarchicalName { get; private set; }
        public string ContainedName { get; private set; }
        public int ConfigVersion { get; private set; }
        public Template Template { get; private set; }
        public string DerivedFromName { get; private set; }
        public string HostName { get; private set; }
        public string AreaName { get; private set; }
        public string ContainerName { get; private set; }

        public void SetTagName(string tagName)
        {
            TagName = tagName;
            //Template.SetPrimitiveValue(tagName, nameof(TagName));
        }

        public void SetContainedName(string containedName)
        {
            ContainedName = containedName;
            //Template.SetPrimitiveValue(containedName, nameof(ContainedName));
        }

        public void SetHierarchicalName(string hierarchicalName)
        {
            HierarchicalName = hierarchicalName;
            //Template.SetPrimitiveValue(hierarchicalName, nameof(HierarchicalName));
        }

        public virtual XElement Serialize()
        {
            var root = new XElement(nameof(ObjectBase));
            root.Add(new XAttribute(nameof(TagName), TagName ?? string.Empty));
            root.Add(new XAttribute(nameof(HierarchicalName), HierarchicalName ?? string.Empty));
            root.Add(new XAttribute(nameof(ContainedName), ContainedName ?? string.Empty));
            root.Add(new XAttribute(nameof(ConfigVersion), ConfigVersion));
            root.Add(new XAttribute(nameof(Template), Template.Name ?? string.Empty));
            root.Add(new XAttribute(nameof(DerivedFromName), DerivedFromName ?? string.Empty));
            root.Add(new XAttribute(nameof(HostName), HostName ?? string.Empty));
            root.Add(new XAttribute(nameof(AreaName), AreaName ?? string.Empty));
            root.Add(new XAttribute(nameof(ContainerName), ContainerName ?? string.Empty));
            
            return root;
        }

        public virtual bool Equals(ObjectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TagName == other.TagName && 
                   ContainedName == other.ContainedName &&
                   HierarchicalName == other.HierarchicalName &&
                   ConfigVersion == other.ConfigVersion &&
                   Equals(Template, other.Template) &&
                   DerivedFromName == other.DerivedFromName &&
                   HostName == other.HostName &&
                   AreaName == other.AreaName &&
                   ContainerName == other.ContainerName;
        }

        public IEnumerable<Difference> DiffersFrom(ObjectBase other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.TagName));
            differences.AddRange(Difference.Between(this, other, x => x.ContainedName));
            differences.AddRange(Difference.Between(this, other, x => x.HierarchicalName));
            differences.AddRange(Difference.Between(this, other, x => x.ConfigVersion));
            differences.AddRange(Difference.Between(this, other, x => x.Template));
            differences.AddRange(Difference.Between(this, other, x => x.DerivedFromName));
            differences.AddRange(Difference.Between(this, other, x => x.HostName));
            differences.AddRange(Difference.Between(this, other, x => x.AreaName));
            differences.AddRange(Difference.Between(this, other, x => x.ContainerName));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ObjectBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TagName != null ? TagName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HierarchicalName != null ? HierarchicalName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ContainedName != null ? ContainedName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ConfigVersion;
                hashCode = (hashCode * 397) ^ (Template != null ? Template.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DerivedFromName != null ? DerivedFromName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HostName != null ? HostName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AreaName != null ? AreaName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ContainerName != null ? ContainerName.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ObjectBase left, ObjectBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ObjectBase left, ObjectBase right)
        {
            return !Equals(left, right);
        }
    }
}