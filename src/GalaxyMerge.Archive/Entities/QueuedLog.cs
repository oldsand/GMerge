using System;

namespace GalaxyMerge.Archive.Entities
{
    public class QueuedLog
    {
        private QueuedLog()
        {
        }
        
        public QueuedLog(int changeLogId, int objectId)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            QueuedOn = DateTime.Now;
            IsProcessing = false;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public DateTime QueuedOn { get; private set; }
        public bool IsProcessing { get; set; }
    }
}