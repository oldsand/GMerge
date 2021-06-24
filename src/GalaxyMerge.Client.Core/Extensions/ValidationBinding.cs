using System.Windows.Data;

namespace GalaxyMerge.Client.Core.Extensions
{
    public class ValidationBinding : Binding
    {
        public ValidationBinding()
        {
            NotifyOnValidationError = true;
            ValidatesOnDataErrors = true;
        }

        public ValidationBinding(object o)
        {
            NotifyOnValidationError = true;
            ValidatesOnDataErrors = true;
        }
    }
}