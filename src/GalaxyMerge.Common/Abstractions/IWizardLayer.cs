using System.Collections.Generic;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IWizardLayer : IXmlConvertible<IWizardLayer>
    {
        string Name { get; }
        string Rule { get; }
        IEnumerable<IWizardAssociation> Associations { get; }
    }
}