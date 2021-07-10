using System.ComponentModel;

namespace GClient.Wrappers.Base
{
    public interface ITrackingObject : IRevertibleChangeTracking, IValidatableChangeTracking,
        IRequiredPropertyTracking, INotifyPropertyChanged
    {
    }
}