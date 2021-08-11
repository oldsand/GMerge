using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

// Object will be deserialized via xml
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GCommon.Primitives
{
    public class WizardOption : IXSerializable
    {
        public string Name { get; set; }
        public WizardOptionType OptionType { get; set; }
        public string Rule { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public IEnumerable<WizardChoice> Choices { get; set; }

        public WizardOption Materialize(XElement element)
        {
            Name = element.Attribute(nameof(Name))?.Value;
            OptionType = (WizardOptionType) Enum.Parse(typeof(WizardOptionType), element.Name.ToString());
            Rule = element.Attribute(nameof(Rule))?.Value;
            Description = element.Attribute(nameof(Description))?.Value;
            DefaultValue = element.Attribute(nameof(DefaultValue))?.Value;
            Choices = element.Descendants("Choice").Select(c => new WizardChoice().Materialize(c)).ToList();
            return this;
        }

        public XElement Serialize()
        {
            var element = new XElement(OptionType.ToString());
            element.Add(new XAttribute(nameof(Name), Name));
            element.Add(new XAttribute(nameof(Rule), Rule));
            element.Add(new XAttribute(nameof(Description), Description));
            element.Add(new XAttribute(nameof(DefaultValue), DefaultValue));
            element.Add(Choices.Select(c => c.Serialize()));
            return element;
        }
    }
}