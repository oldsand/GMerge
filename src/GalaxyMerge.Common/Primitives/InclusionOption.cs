using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Primitives
{
    public class InclusionOption : Enumeration
    {
        private InclusionOption()
        {
        }

        private InclusionOption(int id, string name) : base(id, name)
        {
        }

        public static readonly InclusionOption None = new InclusionOption(0, "None");
        public static readonly InclusionOption All = new InclusionOption(1, "All");
        public static readonly InclusionOption Select = new InclusionOption(2, "Select");
        public static readonly InclusionOption New = new InclusionOption(3, "New");
        public static readonly InclusionOption Existing = new InclusionOption(4, "Existing");
        public static readonly InclusionOption Derived = new InclusionOption(5, "Derived");
    }
}