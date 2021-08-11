using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class ArchiveState : SmartEnum<ArchiveState>
    {
        private ArchiveState(string name, int value) : base(name, value)
        {
        }

        public static readonly ArchiveState New = new ArchiveState("New", 0);
        public static readonly ArchiveState Processing = new ArchiveState("Processing", 1);
        public static readonly ArchiveState Archived = new ArchiveState("Archived", 0);
        public static readonly ArchiveState Failed = new ArchiveState("Failed", 2);
    }
}