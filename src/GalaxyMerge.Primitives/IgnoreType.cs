using GalaxyMerge.Core;

namespace GalaxyMerge.Primitives
{
    public class IgnoreType : Enumeration
    {
        private IgnoreType()
        {
        }

        private IgnoreType(int id, string name) : base(id, name)
        {
        }

        public static readonly IgnoreType Pattern = new IgnoreType(0, "Pattern");
        public static readonly IgnoreType Derived = new IgnoreType(1, "Derived");
        public static readonly IgnoreType Folder = new IgnoreType(2, "Folder");
        
        /*private class PatternIgnore : IgnoreType
        {
            public PatternIgnore() : base(0, "Pattern")
            {
            }

            public bool IsIgnorable(string text, string pattern)
            {
                
            }
        }*/
    }
}