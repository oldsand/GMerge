using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Core.Abstractions;
using GCommon.Core.Enumerations;
using GCommon.Differencing;
using GCommon.Differencing.Abstractions;

namespace GCommon.Core
{
    public class WizardOption : IXSerializable, IDifferentiable<WizardOption>
    {
        private WizardOption(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            OptionType = (WizardOptionType) Enum.Parse(typeof(WizardOptionType), element.Name.ToString());
            Rule = element.Attribute(nameof(Rule))?.Value;
            Description = element.Attribute(nameof(Description))?.Value;
            DefaultValue = element.Attribute(nameof(DefaultValue))?.Value;
            Choices = element.Descendants("Choice").Select(WizardChoice.Materialize).ToList();
        }

        public string Name { get; set; }
        public WizardOptionType OptionType { get; set; }
        public string Rule { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public IEnumerable<WizardChoice> Choices { get; set; }

        public static WizardOption Materialize(XElement element)
        {
            return new WizardOption(element);
        }

        public XElement Serialize()
        {
            var element = new XElement(OptionType.Name);
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));
            element.Add(new XAttribute(nameof(Description), Description));
            element.Add(new XAttribute(nameof(DefaultValue), DefaultValue));
            element.Add(Choices.Select(c => c.Serialize()));
            return element;
        }
        
        public bool Equals(WizardOption other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && OptionType == other.OptionType && Rule == other.Rule &&
                   Description == other.Description && DefaultValue == other.DefaultValue &&
                   Equals(Choices, other.Choices);
        }

        public IEnumerable<Difference> DiffersFrom(WizardOption other)
        {
            var differences = new List<Difference>();

            differences.AddRange(Difference.Between(this, other, x => x.Name));
            differences.AddRange(Difference.Between(this, other, x => x.OptionType));
            differences.AddRange(Difference.Between(this, other, x => x.Rule));
            differences.AddRange(Difference.Between(this, other, x => x.Description));
            differences.AddRange(Difference.Between(this, other, x => x.DefaultValue));
            differences.AddRange(Difference.BetweenCollection(Choices, other.Choices, x => x.Name));

            return differences;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WizardOption) obj);
        }

        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) OptionType;
                hashCode = (hashCode * 397) ^ (Rule != null ? Rule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DefaultValue != null ? DefaultValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Choices != null ? Choices.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(WizardOption left, WizardOption right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WizardOption left, WizardOption right)
        {
            return !Equals(left, right);
        }
    }
}