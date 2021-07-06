using System;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
{
    public class QueuedEntry
    {
        private QueuedEntry()
        {
        }
        
        public QueuedEntry(int changeLogId, int objectId, int operationId, DateTime changeOn)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            OperationId = operationId;
            ChangeOn = changeOn;
            QueuedOn = DateTime.Now;
            State = QueueState.New;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public int OperationId { get; private set; }
        public DateTime ChangeOn { get; private set; }
        public DateTime QueuedOn { get; private set; }
        public QueueState State { get; set; }
    }
}