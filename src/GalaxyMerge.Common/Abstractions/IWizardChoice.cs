using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface IWizardChoice : IXmlConvertible<IWizardChoice>
    {
        string Name { get; }
        string Rule { get; }
    }
}