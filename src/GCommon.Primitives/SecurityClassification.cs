using GCommon.Primitives.Base;

namespace GCommon.Primitives
{
    public class SecurityClassification : Enumeration
    {
        public static readonly SecurityClassification Undefined = new SecurityClassification(-1, "Undefined");
        public static readonly SecurityClassification FreeAccess = new SecurityClassification(0, "FreeAccess");
        public static readonly SecurityClassification Operate = new SecurityClassification(1, "Operate");
        public static readonly SecurityClassification SecureWrite = new SecurityClassification(2, "SecureWrite");
        public static readonly SecurityClassification VerifiedWrite = new SecurityClassification(3, "VerifiedWrite");
        public static readonly SecurityClassification Tune = new SecurityClassification(4, "Tune");
        public static readonly SecurityClassification Configure = new SecurityClassification(5, "Configure");
        public static readonly SecurityClassification ViewOnly = new SecurityClassification(6, "ViewOnly");

        private SecurityClassification()
        {
        }
        private SecurityClassification(int id, string name) : base (id, name)
        {
        }
    }
}