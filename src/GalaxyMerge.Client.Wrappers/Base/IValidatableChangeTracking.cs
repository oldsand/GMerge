using System.ComponentModel;

namespace GalaxyMerge.Client.Wrappers.Base
{
    public interface IValidatableChangeTracking : IRevertibleChangeTracking, INotifyPropertyChanged
    {
        bool IsValid { get; }
    }
}