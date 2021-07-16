using System;

namespace GCommon.Archiving.Entities
{
    public class QueuedLog
    {
        private QueuedLog()
        {
        }
        
        public QueuedLog(int changeLogId)
        {
            ChangeLogId = changeLogId;
            QueuedOn = DateTime.Now;
        }
        
        public int ChangeLogId { get; private set; }
        public DateTime QueuedOn { get; private set; }
    }
}