using System;
using GCommon.Primitives;

namespace GCommon.Archiving.Entities
{
    public class QueuedEntry
    {
        private QueuedEntry()
        {
        }
        
        public QueuedEntry(int changeLogId, int objectId)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            QueuedOn = DateTime.Now;
            State = QueueState.New;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; set; }
        public DateTime QueuedOn { get; private set; }
        public QueueState State { get; set; }
    }
}