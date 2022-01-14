using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class OperationType : SmartEnum<OperationType>
    {
        private OperationType(string name, int value) : base(name, value)
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

        public static readonly OperationType Unchanged = new OperationType( "Unchanged", 0);
        public static readonly OperationType Creation = new OperationType("Creation", 1);
        public static readonly OperationType Modification = new OperationType("Modification", 2);
        public static readonly OperationType StateChange = new OperationType("StateChange", 3);
        public static readonly OperationType Software = new OperationType("Software", 4);
    }
}