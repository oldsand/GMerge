using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class SecurityClassification : SmartEnum<SecurityClassification>
    {
        private SecurityClassification(string name, int value) : base(name, value)
        {
        }

        public static readonly SecurityClassification Undefined = new SecurityClassification("Undefined", -1);
        public static readonly SecurityClassification FreeAccess = new SecurityClassification("FreeAccess", 0);
        public static readonly SecurityClassification Operate = new SecurityClassification("Operate", 1);
        public static readonly SecurityClassification SecureWrite = new SecurityClassification("SecureWrite", 2);
        public static readonly SecurityClassification VerifiedWrite = new SecurityClassification("VerifiedWrite", 3);
        public static readonly SecurityClassification Tune = new SecurityClassification("Tune", 4);
        public static readonly SecurityClassification Configure = new SecurityClassification("Configure", 5);
        public static readonly SecurityClassification ViewOnly = new SecurityClassification("ViewOnly", 6);
    }
}