using GalaxyMerge.Primitives.Base;

namespace GalaxyMerge.Primitives
{
    public class Operation : Enumeration
    {
        private Operation()
        {
        }

        private Operation(int id, string name) : base(id, name)
        {
        }

        public static readonly Operation CheckInSuccess = new Operation(0, "CheckInSuccess");
        public static readonly Operation CheckInFailed = new Operation(1, "CheckInFailed");
        public static readonly Operation CheckOutSuccess = new Operation(2, "CheckOutSuccess");
        public static readonly Operation CheckOutFailed = new Operation(3, "CheckOutFailed");
        public static readonly Operation UndoCheckOutSuccess = new Operation(4, "UndoCheckOutSuccess");
        public static readonly Operation UndoCheckOutFailed = new Operation(5, "UndoCheckOutFailed");
        public static readonly Operation DeploySuccess = new Operation(6, "DeploySuccess");
        public static readonly Operation DeployFailed = new Operation(7, "DeployFailed");
        public static readonly Operation UnDeploySuccess = new Operation(8, "UnDeploySuccess");
        public static readonly Operation UnDeployFailed = new Operation(9, "UnDeployFailed");
        public static readonly Operation AssignSuccess = new Operation(10, "AssignSuccess");
        public static readonly Operation AssignFailed = new Operation(11, "AssignFailed");
        public static readonly Operation UnAssignSuccess = new Operation(12, "UnAssignSuccess");
        public static readonly Operation UnAssignFailed = new Operation(13, "UnAssignFailed");
        public static readonly Operation CreateInstance = new Operation(14, "CreateInstance");
        public static readonly Operation PublishSuccess = new Operation(15, "PublishSuccess");
        public static readonly Operation PublishFailed = new Operation(16, "PublishFailed");
        public static readonly Operation UnPublishSuccess = new Operation(17, "UnPublishSuccess");
        public static readonly Operation UnPublishFailed = new Operation(18, "UnPublishFailed");
        public static readonly Operation SetAssociationSuccess = new Operation(19, "SetAssociationSuccess");
        public static readonly Operation SetAssociationFailed = new Operation(20, "SetAssociationFailed");
        public static readonly Operation SetDefaultGraphicSuccess = new Operation(21, "SetDefaultGraphicSuccess");
        public static readonly Operation SetUserPreferenceSuccess = new Operation(22, "SetUserPreferenceSuccess");
        public static readonly Operation OverrideCheckOutSuccess = new Operation(23, "OverrideCheckOutSuccess");
        public static readonly Operation OverrideCheckOutFailed = new Operation(24, "OverrideCheckOutFailed");
        public static readonly Operation SU_UpgradeStarted = new Operation(25, "SU_UpgradeStarted");
        public static readonly Operation SU_PackageValidateSuccess = new Operation(26, "SU_PackageValidateSuccess");
        public static readonly Operation SU_PackageValidateFailed = new Operation(27, "SU_PackageValidateFailed");
        public static readonly Operation SU_RuntimeUpgradeRequired = new Operation(28, "SU_RuntimeUpgradeRequired");
        public static readonly Operation Upload = new Operation(29, "Upload");
        public static readonly Operation RenameTagName = new Operation(30, "RenameTagName");
        public static readonly Operation RenameContainName = new Operation(31, "RenameContainName");
        public static readonly Operation SaveObject = new Operation(32, "SaveObject");
        public static readonly Operation UpgradeObject = new Operation(33, "UpgradeObject");
        public static readonly Operation CreateDerivedTemplate = new Operation(34, "CreateDerivedTemplate");
        public static readonly Operation Rename = new Operation(35, "Rename");
        public static readonly Operation RenameContainedName = new Operation(36, "RenameContainedName");
        public static readonly Operation ModifiedAutomationObjectOnly = new Operation(37, "ModifiedAutomationObjectOnly");
        public static readonly Operation ModifiedGraphicOnly = new Operation(38, "ModifiedGraphicOnly");
        public static readonly Operation ModifiedGraphicAndAutomationObject = new Operation(39, "ModifiedGraphicAndAutomationObject");
    }
}