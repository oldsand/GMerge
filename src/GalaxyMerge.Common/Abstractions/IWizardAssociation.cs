using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IWizardAssociation : IXmlConvertible<IWizardAssociation>
    {
        string Name { get; }
        WizardAssociationType AssociationType { get; }
    }
}