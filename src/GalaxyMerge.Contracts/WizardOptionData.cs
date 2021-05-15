using System.Collections.Generic;
using GalaxyMerge.Common.Primitives;

namespace GalaxyMerge.Contracts
{
    public class WizardOptionData
    {
        public string Name { get; set; }
        public WizardOptionType OptionType { get; set; }
        public string Rule { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public IEnumerable<WizardChoiceData> Choices { get; set; }
    }
}