using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Core.Abstractions;
using GCommon.Core.Enumerations;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Core
{
    public class ArchestraObject : ObjectBase, IDifferentiable<ArchestraObject>, IDuplicable<ArchestraObject>
    {
        public ArchestraObject(string tagName, Template template, string derivedFrom = null, int version = 1,
            IEnumerable<ArchestraAttribute> attributes = null) 
            : base(tagName, template, derivedFrom, version)
        {
            Attributes = attributes;
        }

        public ArchestraObject(string tagName, string hierarchicalName, string containedName, int configVersion,
            Template template, string derivedFromName, string hostName, string areaName, string containerName,
            IEnumerable<ArchestraAttribute> attributes) 
            : base(tagName, hierarchicalName, containedName, configVersion, template, derivedFromName, hostName,
                areaName, containerName)
        {
            Attributes = attributes;
        }

        public ArchestraObject(XElement element) : base(element)
        {
            Attributes = element.Descendants(nameof(Attributes)).Select(ArchestraAttribute.Materialize);
        }

        public IEnumerable<ArchestraAttribute> Attributes { get; private set; }
       

        public bool HasAttribute(string name)
        {
            return Attributes.Any(x => x.Name == name);
        }

        public ArchestraAttribute GetAttribute(string name)
        {
            return Attributes.SingleOrDefault(a => a.Name == name);
        }

        public void AddAttribute(ArchestraAttribute attribute)
        {
            //todo need to implement this?
        }

        public static ArchestraObject Materialize(XElement element)
        {
            return new ArchestraObject(element);
        }

        public override XElement Serialize()
        {
            var root = base.Serialize();

            if (Attributes != null)
                root.Add(new XElement(nameof(Attributes), Attributes.Select(a => a.Serialize())));

            return root;
        }

        public ArchestraObject Duplicate()
        {
            return Materialize(Serialize());
        }

        public bool Equals(ArchestraObject other)
        {
            return base.Equals(other);
        }

        public IEnumerable<Difference> DiffersFrom(ArchestraObject other)
        {
            var differences = base.DiffersFrom(other).ToList();

            differences.AddRange(Difference.BetweenCollection(Attributes, other.Attributes, a => a.Name));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ArchestraObject)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ArchestraObject left, ArchestraObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ArchestraObject left, ArchestraObject right)
        {
            return !Equals(left, right);
        }
    }
}