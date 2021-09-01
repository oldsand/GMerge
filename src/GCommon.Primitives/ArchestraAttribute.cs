using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class ArchestraAttribute : IXSerializable, IDifferentiable<ArchestraAttribute>
    {
        private ArchestraAttribute(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            DataType = DataType.FromName(element.Attribute(nameof(DataType))?.Value);
            Category = AttributeCategory.FromName(element.Attribute(nameof(Category))?.Value);
            Security = SecurityClassification.FromName(element.Attribute(nameof(Security))?.Value);
            Locked = LockType.FromName(element.Attribute(nameof(Locked))?.Value);
            ArrayCount = Convert.ToInt32(element.Attribute(nameof(ArrayCount))?.Value);
            Value = ReadValue(element);
        }
        
        public ArchestraAttribute(string name, DataType dataType, 
            AttributeCategory category = null,
            SecurityClassification security = null, 
            LockType locked = null, 
            object value = null, 
            int arrayCount = -1)
        {
            Name = name;
            DataType = dataType;
            Category = category ?? AttributeCategory.Writeable_UC_Lockable;
            Security = security ?? SecurityClassification.Operate;
            Locked = locked ?? LockType.Unlocked;
            Value = value ?? dataType.DefaultValue;
            ArrayCount = arrayCount;
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
            return new ArchestraAttribute(element);
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
            element.Add(WriteValue());
            return element;
        }

        public void SetCategory(AttributeCategory category)
        {
            category.When(AttributeCategory.Calculated).Then(() => Category = category);
            category.When(AttributeCategory.CalculatedRetentive).Then(() => Category = category);
            category.When(AttributeCategory.Writeable_UC_Lockable).Then(() => Category = category);
        }

        public void SetLocked(LockType lockType)
        {
            if (Locked.Equals(LockType.InParent)) return;

            Locked = lockType;
        }

        public bool Equals(ArchestraAttribute other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Equals(DataType, other.DataType) && Equals(Category, other.Category) &&
                   Equals(Security, other.Security) && Equals(Locked, other.Locked) && Equals(Value, other.Value) &&
                   ArrayCount == other.ArrayCount;
        }

        public IEnumerable<Difference> DiffersFrom(ArchestraAttribute other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.Name));
            differences.AddRange(Difference.Between(this, other, x => x.DataType));
            differences.AddRange(Difference.Between(this, other, x => x.Category));
            differences.AddRange(Difference.Between(this, other, x => x.Security));
            differences.AddRange(Difference.Between(this, other, x => x.Locked));
            differences.AddRange(Difference.Between(this, other, x => x.Value));
            differences.AddRange(Difference.Between(this, other, x => x.ArrayCount));

            return differences;
        }

        public override string ToString()
        {
            return $"Name: {Name}; Type: {DataType}; Value: {Value}";
        }

        private static object ReadValue(XElement element)
        {
            var dataType = DataType.FromName(element.Attribute(nameof(DataType))?.Value);
            if (dataType == null) throw new InvalidOperationException();

            return Convert.ToInt32(element.Attribute(nameof(ArrayCount))?.Value) == -1
                ? dataType.Parse(element.Value.Trim())
                : element.Descendants("Element").Select(e => dataType.Parse(e.Value.Trim())).ToArray();
        }

        private XElement WriteValue()
        {
            var value = new XElement("Value");
            if (Value == null) return value;

            if (ArrayCount == -1)
                value.Add(new XCData(Value.ToString()));
            else
            {
                var array = ((IEnumerable)Value).Cast<object>().Select(s => s.ToString());
                foreach (var item in array)
                    value.Add(new XElement("Element", new XCData(item)));
            }

            return value;
        }
    }
}