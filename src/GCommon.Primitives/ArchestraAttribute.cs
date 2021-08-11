using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class ArchestraAttribute : IXSerializable
    {
        private ArchestraAttribute()
        {
            Name = "Attribute001";
            DataType = DataType.Boolean;
            Category = AttributeCategory.Writeable_UC_Lockable;
            Security = SecurityClassification.Operate;
            Locked = LockType.Unlocked;
            Value = DataType.Boolean.DefaultValue;
            ArrayCount = -1;
        }

        public ArchestraAttribute(string name, DataType dataType)
        {
            Name = name;
            DataType = dataType;
            Category = AttributeCategory.Writeable_UC_Lockable;
            Security = SecurityClassification.Operate;
            Locked = LockType.Unlocked;
            Value = dataType.DefaultValue;
            ArrayCount = -1;
        }

        public string Name { get; set; }
        public DataType DataType { get; set; }
        public AttributeCategory Category { get; set; }
        public SecurityClassification Security { get; set; }
        public LockType Locked { get; set; }
        public object Value { get; set; }
        public int ArrayCount { get; set; }

        public static ArchestraAttribute Materialize(XElement element)
        {
            return new ArchestraAttribute
            {
                Name = element.Attribute(nameof(Name))?.Value,
                DataType = DataType.FromName(element.Attribute(nameof(DataType))?.Value),
                Category = AttributeCategory.FromName(element.Attribute(nameof(Category))?.Value),
                Security = SecurityClassification.FromName(element.Attribute(nameof(Security))?.Value),
                Locked = LockType.FromName(element.Attribute(nameof(Locked))?.Value),
                ArrayCount = Convert.ToInt32(element.Attribute(nameof(ArrayCount))?.Value),

                /*Value = Convert.ToInt32(element.Attribute(nameof(ArrayCount))?.Value) == -1
                    ? element.Value.Trim()
                    : element.Descendants("Element").Select(e => e.Value.Trim()).ToArray()*/
            };
        }

        public XElement Serialize()
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