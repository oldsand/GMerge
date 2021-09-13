using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class LockType : SmartEnum<LockType>
    {
        private LockType(string name, int value) : base(name, value)
        {
        }

        public static readonly LockType Undefined = new LockType("Undefined", -1);
        public static readonly LockType Unlocked = new LockType("Unlocked", 0);
        public static readonly LockType InMe = new LockType("InMe", 1);
        public static readonly LockType InParent = new LockType("InParent", 2);
    }
}