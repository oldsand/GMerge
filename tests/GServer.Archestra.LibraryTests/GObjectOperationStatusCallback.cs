using System;
using System.Collections;
using ArchestrA.Core;

namespace GServer.Archestra.IntegrationTests
{
    internal class GObjectOperationStatusCallback : WaitableCallback, IGObjectOperationStatus, IPackageManagerCallback
    {
        private string operationName;
        private string completedOperationName;
        private ArrayList results = new ArrayList();
        private EPACKAGEOPERATIONSTATUS lastOperationStatus = EPACKAGEOPERATIONSTATUS.ePackageFailure;
        private FAILURESTATUS lastFailureStatus;
        private string lastOperationMessage = "No message received";
        private Hashtable listTemplateResults = new Hashtable();
        private int newFSobjectId;
        private bool showProgress;
        private bool totalOperationSuccess = true;

        public Hashtable ListTemplateResults => this.listTemplateResults;

        public bool TotalOperationSuccess => this.totalOperationSuccess;

        public int NewFSObjectId
        {
            get => this.newFSobjectId;
            set => this.newFSobjectId = value;
        }

        public EPACKAGEOPERATIONSTATUS LastOperationStatus => this.lastOperationStatus;

        public string LastOperationMessage => this.lastOperationMessage;

        public ArrayList Results => this.results;

        public FAILURESTATUS LastFailureStatus => this.lastFailureStatus;

        public string CompletedOperationName
        {
            get => this.completedOperationName;
            set => this.completedOperationName = value;
        }

        public bool ShowProgress
        {
            get => this.showProgress;
            set => this.showProgress = value;
        }

        public void OnOperationComplete(int hint) => this.WaitHandle.Set();

        public void OnOperationUpdate(FAILURESTATUS operationSatus, int hint)
        {
            this.results.Add((object)operationSatus);
            this.lastFailureStatus = operationSatus;
            if (operationSatus.reasoncode == ERRORCODE.eNoError || !this.totalOperationSuccess)
                return;
            this.totalOperationSuccess = false;
        }

        public void OnOperationCancel(int hint) => this.WaitHandle.Set();

        public void OnOperationMessage(int hint, string message)
        {
        }

        public void OnOperationPercentageComplete(int hint, int percentComplete)
        {
        }

        public void OnDoDragDropResult(
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage)
        {
            this.WaitHandle.Set();
        }

        public void OnAssignResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
            this.WaitHandle.Set();
        }

        public void OnListObjectsResult(int hint, IGObjectInfo gobjectInfo)
        {
        }

        public void OnCascadeUndeployUpdate(
            ref int fsobjectId,
            int hint,
            EPACKAGEUNDEPLOYSTATUS operationStatus,
            string operationStatusMessage)
        {
            this.WaitHandle.Set();
        }

        public void OnUploadResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] pailedObjects)
        {
            this.WaitHandle.Set();
        }

        public void OnReDeployResult(
            int hint,
            EPACKAGEUNDEPLOYSTATUS undeployStatus,
            string undeployStatusMessage,
            EPACKAGEOPERATIONSTATUS deployStatus,
            string deployStatusMessage)
        {
            this.WaitHandle.Set();
        }

        public void OnListObjectsByCategoryResult(int hint, IGObjectInfo gobjectInfo)
        {
        }

        public void OnCheckInResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
            this.WaitHandle.Set();
        }

        public void OnListToolsetsResult(int hint, object toolsetList)
        {
        }

        public void OnListTemplatesResult(int hint, IGObjectInfo gobjectInfo)
        {
            if (gobjectInfo == null)
                return;
            this.ListTemplateResults.Add((object)gobjectInfo.Id, (object)gobjectInfo);
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
            this.WaitHandle.Set();
        }

        public void OnCreateFsObjectResult(
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage,
            ref int newFsID)
        {
            this.newFSobjectId = newFsID;
            this.lastOperationStatus = operationStatus;
            this.lastOperationMessage = operationStatusMessage;
            this.WaitHandle.Set();
        }

        public void OnUndeployResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
            this.WaitHandle.Set();
        }

        public void OnCheckOutResult(
            int hint,
            int[] objectsSuccessfullyProcessed,
            FAILURESTATUS[] failedObjects)
        {
            this.WaitHandle.Set();
        }

        public void OnCascadeUndeployComplete(int hint) => throw new NotImplementedException();

        public void OnCascadeDeployComplete(int hint) => throw new NotImplementedException();

        public void OnCascadeDeployUpdate(
            ref int fsobjectId,
            int hint,
            EPACKAGEOPERATIONSTATUS operationStatus,
            string operationStatusMessage)
        {
            throw new NotImplementedException();
        }

        public void OnDynamicListObjectsResult(int hint, int unused, IGObjectInfo gobjectInfo) =>
            throw new NotImplementedException();

        public void Initialize() => throw new NotImplementedException();

        public void NotifyStatus() => throw new NotImplementedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.WaitHandle.Close();
            base.Dispose(disposing);
        }

        private void SetResult(int hint, EPACKAGEOPERATIONSTATUS status, string message)
        {
            this.lastOperationStatus = status;
            this.lastOperationMessage = message;
        }
    }
}