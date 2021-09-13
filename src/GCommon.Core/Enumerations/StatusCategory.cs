using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class StatusCategory : SmartEnum<StatusCategory>
    {
        private StatusCategory(string name, int value) : base(name, value)
        {
        }
        
        public static readonly StatusCategory Unknown = new StatusCategory("Unknown", -1);
        public static readonly StatusCategory Ok = new StatusCategory("Ok", 0);
        public static readonly StatusCategory Pending = new StatusCategory("Pending", 1);
        public static readonly StatusCategory Warning = new StatusCategory("Warning", 2);
        public static readonly StatusCategory CommunicationError = new StatusCategory("CommunicationError", 3);
        public static readonly StatusCategory ConfigurationError = new StatusCategory("ConfigurationError", 4);
        public static readonly StatusCategory OperationalError = new StatusCategory("OperationalError", 5);
        public static readonly StatusCategory SecurityError = new StatusCategory("SecurityError", 6);
        public static readonly StatusCategory SoftwareError = new StatusCategory("SoftwareError", 7);
        public static readonly StatusCategory OtherError = new StatusCategory("OtherError", 8);
    }
}