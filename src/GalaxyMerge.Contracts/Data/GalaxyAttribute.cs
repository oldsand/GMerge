using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using GalaxyMerge.Common.Abstractions;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Extensions;

// Object will be deserialized via xml
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GalaxyMerge.Contracts.Data
{
    public class GalaxyAttribute : IGalaxyAttribute
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public AttributeCategory Category { get; set; }
        public SecurityClassification Security { get; set; }
        public LockType Locked { get; set; }
        public object Value { get; set; }
        public int ArrayCount { get; set; }

        public IGalaxyAttribute FromXml(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            DataType = Enumeration.FromName<DataType>(element.Attribute(nameof(DataType))?.Value);
            Category = Enumeration.FromName<AttributeCategory>(element.Attribute(nameof(Category))?.Value);
            Security = Enumeration.FromName<SecurityClassification>(element.Attribute(nameof(Security))?.Value);
            Locked = Enumeration.FromName<LockType>(element.Attribute(nameof(Locked))?.Value);
            ArrayCount = Convert.ToInt32(element.Attribute(nameof(ArrayCount))?.Value);

            Value = ArrayCount == -1
                ? element.Value.Trim()
                : element.Descendants("Element").Select(e => e.Value.Trim()).ToArray().ConvertTo<object>();

            return this;
        }

        public XElement ToXml()
        {
            var element = new XElement("Attribute");
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(DataType), DataType.Name));
            element.Add(new XAttribute(nameof(Category), Category.Name));
            element.Add(new XAttribute(nameof(Security), Security.Name));
            element.Add(new XAttribute(nameof(Locked), Locked.Name));
            element.Add(new XAttribute(nameof(ArrayCount), ArrayCount));

            var value = new XElement("Value");
            if (Value != null)
            {
                if (ArrayCount == -1)
                    value.Add(new XCData(Value.ToString()));
                else
                {
                    var array = ((IEnumerable) Value).Cast<object>().Select(s => s.ToString());
                    foreach (var item in array)
                        value.Add(new XElement("Element", new XCData(item)));
                }
            }
            
            element.Add(value);
            return element;
        }
    }
}