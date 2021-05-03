using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Archive.Enum
{
    public class EventType : Enumeration
    {
        private EventType()
        {
        }

        private EventType(int id, string name) : base(id, name)
        {
        }

        public static EventType FromOperation(Operation operation)
        {
            if (operation == Operation.CreateInstance || operation == Operation.CreateDerivedTemplate)
                return Creation;
            if (operation == Operation.CheckInSuccess || operation == Operation.Rename || operation == Operation.AssignSuccess)
                return Modification;

            return StateChange;
        }

        public static readonly EventType Creation = new EventType(0, "Creation");
        public static readonly EventType Modification = new EventType(1, "Modification");
        public static readonly EventType StateChange = new EventType(2, "StateChange");
    }
}