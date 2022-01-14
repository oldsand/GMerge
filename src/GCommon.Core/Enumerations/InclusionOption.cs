using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class InclusionOption : SmartEnum<InclusionOption>
    {
        private InclusionOption(string name, int value) : base(name, value)
        {
        }

        public static readonly InclusionOption None = new InclusionOption("None", 0);
        public static readonly InclusionOption All = new InclusionOption("All", 1);
        public static readonly InclusionOption Select = new InclusionOption("Select", 2);
    }
}