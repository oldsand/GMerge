using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class Quality : SmartEnum<Quality>
    {
        private Quality(string name, int value) : base(name, value)
        {
        }

        public static readonly Quality Unknown = new Quality("Unknown", -1);
        public static readonly Quality Good = new Quality("Good", 0);
        public static readonly Quality Uncertain = new Quality("Uncertain", 1);
        public static readonly Quality Initializing = new Quality("Initializing", 2);
        public static readonly Quality Bad = new Quality("Bad", 3);
    }
}