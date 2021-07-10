using GCommon.Primitives.Base;

namespace GCommon.Primitives
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
            
            if (operation == Operation.CheckInSuccess 
                || operation == Operation.Rename
                || operation == Operation.RenameTagName
                || operation == Operation.RenameContainedName
                || operation == Operation.RenameContainName
                || operation == Operation.SaveObject
                || operation == Operation.ModifiedGraphicOnly
                || operation == Operation.ModifiedAutomationObjectOnly
                || operation == Operation.ModifiedGraphicAndAutomationObject
                || operation == Operation.AssignSuccess
                || operation == Operation.UnAssignSuccess)
                return Modification;

            if (operation == Operation.Upload 
                || operation == Operation.DeploySuccess
                || operation == Operation.UnDeploySuccess)
                return StateChange;

            if (operation == Operation.SU_UpgradeStarted
                || operation == Operation.UpgradeObject
                || operation == Operation.PublishSuccess
                || operation == Operation.UnPublishSuccess
                || operation == Operation.SU_PackageValidateSuccess
                || operation == Operation.SU_RuntimeUpgradeRequired)
                return Software;

            return Unchanged;
        }

        public static readonly OperationType Unchanged = new OperationType(-1, "Unchanged");
        public static readonly OperationType Creation = new OperationType(0, "Creation");
        public static readonly OperationType Modification = new OperationType(1, "Modification");
        public static readonly OperationType StateChange = new OperationType(2, "StateChange");
        public static readonly OperationType Software = new OperationType(3, "Software");
    }
}