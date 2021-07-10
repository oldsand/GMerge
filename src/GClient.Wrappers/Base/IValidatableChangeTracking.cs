using System.ComponentModel;

namespace GClient.Wrappers.Base
{
    public interface IValidatableChangeTracking : IChangeTracking
    {
        bool IsValid { get; }
    }
}