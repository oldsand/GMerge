using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Primitives
{
    public class OperationType : Enumeration
    {
        private OperationType()
        {
        }

        private OperationType(int id, string name) : base(id, name)
        {
        }

        public static OperationType FromOperation(Operation operation)
        {
            if (operation == Operation.CreateInstance || operation == Operation.CreateDerivedTemplate)
                return Creation;
            if (operation == Operation.CheckInSuccess || operation == Operation.Rename || operation == Operation.AssignSuccess)
                return Modification;

            return StateChange;
        }

        public static readonly OperationType Creation = new OperationType(0, "Creation");
        public static readonly OperationType Modification = new OperationType(1, "Modification");
        public static readonly OperationType StateChange = new OperationType(2, "StateChange");
    }
}