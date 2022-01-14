using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class Operation : SmartEnum<Operation>
    {
        private Operation(string name, int value) : base(name, value)
        {
        }

        public static readonly Operation CheckInSuccess = new Operation("CheckInSuccess", 0);
        public static readonly Operation CheckInFailed = new Operation("CheckInFailed", 1);
        public static readonly Operation CheckOutSuccess = new Operation("CheckOutSuccess", 2);
        public static readonly Operation CheckOutFailed = new Operation("CheckOutFailed", 3);
        public static readonly Operation UndoCheckOutSuccess = new Operation("UndoCheckOutSuccess", 4);
        public static readonly Operation UndoCheckOutFailed = new Operation("UndoCheckOutFailed", 5);
        public static readonly Operation DeploySuccess = new Operation("DeploySuccess", 6);
        public static readonly Operation DeployFailed = new Operation("DeployFailed", 7);
        public static readonly Operation UnDeploySuccess = new Operation("UnDeploySuccess", 8);
        public static readonly Operation UnDeployFailed = new Operation("UnDeployFailed", 9);
        public static readonly Operation AssignSuccess = new Operation( "AssignSuccess", 10);
        public static readonly Operation AssignFailed = new Operation( "AssignFailed", 11);
        public static readonly Operation UnAssignSuccess = new Operation( "UnAssignSuccess", 12);
        public static readonly Operation UnAssignFailed = new Operation( "UnAssignFailed", 13);
        public static readonly Operation CreateInstance = new Operation( "CreateInstance", 14);
        public static readonly Operation PublishSuccess = new Operation( "PublishSuccess", 15);
        public static readonly Operation PublishFailed = new Operation( "PublishFailed", 16);
        public static readonly Operation UnPublishSuccess = new Operation( "UnPublishSuccess", 17);
        public static readonly Operation UnPublishFailed = new Operation( "UnPublishFailed", 18);
        public static readonly Operation SetAssociationSuccess = new Operation( "SetAssociationSuccess", 19);
        public static readonly Operation SetAssociationFailed = new Operation( "SetAssociationFailed", 20);
        public static readonly Operation SetDefaultGraphicSuccess = new Operation( "SetDefaultGraphicSuccess", 21);
        public static readonly Operation SetUserPreferenceSuccess = new Operation( "SetUserPreferenceSuccess", 22);
        public static readonly Operation OverrideCheckOutSuccess = new Operation( "OverrideCheckOutSuccess", 23);
        public static readonly Operation OverrideCheckOutFailed = new Operation( "OverrideCheckOutFailed", 24);
        public static readonly Operation SU_UpgradeStarted = new Operation( "SU_UpgradeStarted", 25);
        public static readonly Operation SU_PackageValidateSuccess = new Operation( "SU_PackageValidateSuccess", 26);
        public static readonly Operation SU_PackageValidateFailed = new Operation( "SU_PackageValidateFailed", 27);
        public static readonly Operation SU_RuntimeUpgradeRequired = new Operation( "SU_RuntimeUpgradeRequired", 28);
        public static readonly Operation Upload = new Operation( "Upload", 29);
        public static readonly Operation RenameTagName = new Operation( "RenameTagName", 30);
        public static readonly Operation RenameContainName = new Operation( "RenameContainName", 31);
        public static readonly Operation SaveObject = new Operation( "SaveObject", 32);
        public static readonly Operation UpgradeObject = new Operation( "UpgradeObject", 33);
        public static readonly Operation CreateDerivedTemplate = new Operation( "CreateDerivedTemplate", 34);
        public static readonly Operation Rename = new Operation( "Rename", 35);
        public static readonly Operation RenameContainedName = new Operation( "RenameContainedName", 36);
        public static readonly Operation ModifiedAutomationObjectOnly = new Operation( "ModifiedAutomationObjectOnly", 37);
        public static readonly Operation ModifiedGraphicOnly = new Operation( "ModifiedGraphicOnly", 38);
        public static readonly Operation ModifiedGraphicAndAutomationObject = new Operation( "ModifiedGraphicAndAutomationObject", 39);
    }
}