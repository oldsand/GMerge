using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Primitives
{
    /// <summary>
    /// Enumeration of all known Archestra attribute lock types.
    /// </summary>
    public abstract class LockType : Enumeration
    {
        private LockType()
        {
        }

        private LockType(int id, string name) : base(id, name)
        {
        }
        
        public static readonly LockType Undefined = new UndefinedInternal();
        public static readonly LockType Unlocked = new UnlockedInternal();
        public static readonly LockType InMe = new InMeInternal();
        public static readonly LockType InParent = new InParentInternal();

        private class UndefinedInternal : LockType
        {
            public UndefinedInternal() : base(-1, "Undefined")
            {
            }
        }
        
        private class UnlockedInternal : LockType
        {
            public UnlockedInternal() : base(0, "Unlocked")
            {
            }
        }
        
        private class InMeInternal : LockType
        {
            public InMeInternal() : base(1, "InMe")
            {
            }
        }
        
        private class InParentInternal : LockType
        {
            public InParentInternal() : base(2, "InParent")
            {
            }
        }
    }
}