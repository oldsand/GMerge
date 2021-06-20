using System.ComponentModel;

namespace GalaxyMerge.Client.Wrappers.Base
{
    public interface IValidatableChangeTracking : IChangeTracking
    {
        bool IsValid { get; }
    }
}