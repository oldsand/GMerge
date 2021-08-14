using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class ValidationStatus : SmartEnum<ValidationStatus>
    {
        private ValidationStatus(string name, int value) : base(name, value)
        {
        }

        public static ValidationStatus Unknown = new ValidationStatus("Unknown", -1);
        public static ValidationStatus Good = new ValidationStatus("Good", 0);
        public static ValidationStatus Bad = new ValidationStatus("Bad", 1);
        public static ValidationStatus Warning = new ValidationStatus("Warning", 2);
    }
}