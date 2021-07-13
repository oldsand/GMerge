using GCommon.Primitives.Base;

namespace GCommon.Primitives
{
    public class ArchiveState : Enumeration
    {
        private ArchiveState()
        {
        }

        private ArchiveState(int id, string name) : base(id, name)
        {
        }

        public static readonly ArchiveState New = new ArchiveState(0, "New");
        public static readonly ArchiveState Processing = new ArchiveState(1, "Processing");
        public static readonly ArchiveState Archived = new ArchiveState(0, "Archived");
        public static readonly ArchiveState Failed = new ArchiveState(2, "Failed");
    }
}