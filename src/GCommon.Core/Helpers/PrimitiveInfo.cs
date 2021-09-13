using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Core.Enumerations;

namespace GCommon.Core.Helpers
{
    public abstract class PrimitiveInfo
    {
        private readonly ArchestraAttribute _attribute;

        protected PrimitiveInfo(ArchestraAttribute attribute, string defaultValue)
        {
            _attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));

            if (_attribute.DataType != DataType.BigString)
                throw new InvalidOperationException($"Attribute type is not expected value' {DataType.String}'");

            if (string.IsNullOrEmpty(attribute.Value.ToString()))
                _attribute.Value = defaultValue;
        }
        
        public string Config => _attribute.Value.ToString();
        
        public XElement XConfig => XElement.Parse(_attribute.Value.ToString());

        protected virtual bool IsMatch(XElement first, XElement second)
        {
            return first.FirstAttribute.Value == second.FirstAttribute.Value;
        }

        public virtual void Add(XElement element, string parentName = null)
        {
            if (element == null) return;
            
            var current = XElement.Parse(Config);
            var target = string.IsNullOrEmpty(parentName) ? current : current.Descendants(parentName).Single();

            var node = current.Descendants(element.Name).SingleOrDefault(e => IsMatch(e, element));
            if (node != null) return;

            target.Add(element);

            _attribute.Value = current.ToString();
        }

        public virtual void Add(IEnumerable<XElement> elements)
        {
            foreach (var element in elements)
                Add(element);
        }

        public virtual void Remove(XElement element)
        {
            if (element == null) return;
            
            var current = XElement.Parse(Config);

            var node = current.Descendants(element.Name).SingleOrDefault(e => IsMatch(e, element));
            if (node == null) return;

            node.Remove();

            _attribute.Value = current.ToString();
        }

        public virtual void Remove(IEnumerable<XElement> elements)
        {
            foreach (var element in elements)
                Remove(element);
        }

        public virtual void Replace(XElement element, string parentName = null)
        {
            if (element == null) return;
            
            var current = XElement.Parse(Config);
            var target = string.IsNullOrEmpty(parentName) ? current : current.Descendants(parentName).Single();

            var node = current.Descendants(element.Name).SingleOrDefault(e => IsMatch(e, element));
            node?.Remove();

            target.Add(element);

            _attribute.Value = current.ToString();
        }

        public virtual void Replace(IEnumerable<XElement> elements)
        {
            foreach (var element in elements)
                Replace(element);
        }
    }
}