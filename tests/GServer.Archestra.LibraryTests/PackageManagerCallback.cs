using System.Collections;
using ArchestrA.Core;

namespace GServer.Archestra.IntegrationTests
{
    internal class PackageManagerCallback : WaitableCallback, IPackageManagerCallback
    {
        private EPACKAGEOPERATIONSTATUS lastOperationStatus = EPACKAGEOPERATIONSTATUS.ePackageFailure;
        private string lastOperationMessage = "No message received";
        private Hashtable listTemplateResults = new Hashtable();
        private int newFsObjectId;

        public void OnDoDragDropResult(
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage)
        {
        }

        public void OnAssignResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
        }

        public void OnListObjectsResult(int hint, IGObjectInfo gobjectInfo)
        {
        }

        public void OnCascadeUndeployUpdate(
            ref int objectId,
            int hint,
            EPACKAGEUNDEPLOYSTATUS operationStatus,
            string operationStatusMessage)
        {
        }

        public void OnUploadResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] pailedObjects)
        {
        }

        public void OnReDeployResult(
            int hint,
            EPACKAGEUNDEPLOYSTATUS undeployStatus,
            string undeployStatusMessage,
            EPACKAGEOPERATIONSTATUS deployStatus,
            string deployStatusMessage)
        {
        }

        public void OnListObjectsByCategoryResult(int hint, IGObjectInfo gobjectInfo)
        {
        }

        public void OnCheckInResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
        }

        public void OnListToolsetsResult(int hint, object toolsetList)
        {
        }

        public void OnListTemplatesResult(int hint, IGObjectInfo gobjectInfo)
        {
            if (gobjectInfo == null)
                return;
            this.listTemplateResults.Add((object)gobjectInfo.Id, (object)gobjectInfo);
        }

        public void OnComplete(int hint) => this.WaitHandle.Set();

        public void OnRenameFsObjectResult(
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage)
        {
            this.SetResult(hint, operationStatus, operationStatusMessage);
            this.WaitHandle.Set();
        }

        public void OnUndoCheckOutResult(
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage)
        {
            this.SetResult(hint, operationStatus, operationStatusMessage);
            this.WaitHandle.Set();
        }

        public void OnDeployResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
        }

        public void OnCreateFsObjectResult(
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage,
            ref int newFsID)
        {
            this.newFsObjectId = newFsID;
            this.lastOperationStatus = operationStatus;
            this.lastOperationMessage = operationStatusMessage;
            this.WaitHandle.Set();
        }

        public void OnUndeployResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
        }

        public void OnCheckOutResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
        }

        public void OnCascadeUndeployComplete(int hint)
        {
        }

        public void OnCascadeDeployComplete(int hint)
        {
        }

        public void OnCascadeDeployUpdate(
            ref int objectId,
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage)
        {
        }

        public void OnDynamicListObjectsResult(int hint, int unused, IGObjectInfo gobjectInfo)
        {
        }

        private void SetResult(int hint, EPACKAGEOPERATIONSTATUS status, string message)
        {
            this.lastOperationStatus = status;
            this.lastOperationMessage = message;
            hint = 0;
            this.WaitHandle.Set();
        }
    }
}