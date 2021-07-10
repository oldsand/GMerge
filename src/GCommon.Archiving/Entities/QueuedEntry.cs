using System;
using GCommon.Primitives;

namespace GCommon.Archiving.Entities
{
    public class QueuedEntry
    {
        private QueuedEntry()
        {
        }
        
        public QueuedEntry(int changeLogId, int objectId, int operationId, DateTime changedOn)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            OperationId = operationId;
            ChangedOn = changedOn;
            QueuedOn = DateTime.Now;
            State = QueueState.New;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; set; }
        public int OperationId { get; private set; }
        public DateTime ChangedOn { get; private set; }
        public DateTime QueuedOn { get; private set; }
        public QueueState State { get; set; }
    }
}