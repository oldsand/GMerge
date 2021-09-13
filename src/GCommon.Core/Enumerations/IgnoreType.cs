using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class IgnoreType : SmartEnum<IgnoreType>
    {
        private IgnoreType(string name, int value) : base(name, value)
        {
        }

        public static readonly IgnoreType Pattern = new IgnoreType("Pattern", 0);
        public static readonly IgnoreType Derived = new IgnoreType("Derived", 1);
        public static readonly IgnoreType Folder = new IgnoreType("Folder", 2);
    }
}