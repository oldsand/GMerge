using System.ComponentModel;

namespace GalaxyMerge.Client.Wrappers.Base
{
    public interface ITrackingObject : IRevertibleChangeTracking, IValidatableChangeTracking,
        IRequiredPropertyTracking, INotifyPropertyChanged
    {
    }
}