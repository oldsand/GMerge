using System.Collections.Generic;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IWizardOption : IXmlConvertible<IWizardOption>
    {
        string Name { get; }
        WizardOptionType OptionType { get; }    
        string Rule { get; }
        string Description { get; }
        string DefaultValue { get; }
        IEnumerable<IWizardChoice> Choices { get; }
        
    }
}