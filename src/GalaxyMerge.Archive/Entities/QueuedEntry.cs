using System;
using System.Linq;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archive.Entities
{
    public class QueuedEntry
    {
        private QueuedEntry()
        {
        }
        
        public QueuedEntry(int changeLogId, int objectId, int operationId)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            OperationId = operationId;
            QueuedOn = DateTime.Now;
            State = QueueState.New;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public int OperationId { get; private set; }
        public DateTime QueuedOn { get; private set; }
        public QueueState State { get; set; }
    }
}