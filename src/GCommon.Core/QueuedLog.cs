using System;
using GCommon.Core.Enumerations;

namespace GCommon.Core
{
    public class QueuedLog
    {
        private QueuedLog()
        {
        }

        public QueuedLog(int changeLogId, int objectId, DateTime changedOn, Operation operation, string comment, string userName)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            QueuedOn = DateTime.Now;
            ChangedOn = changedOn;
            Operation = operation;
            Comment = comment;
            UserName = userName;
            State = ArchiveState.New;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public DateTime QueuedOn { get; private set; }
        public DateTime ChangedOn { get; private set; }
        public Operation Operation { get; private set; }
        public string Comment { get; private set; }
        public string UserName { get; private set; }
        public ArchiveState State { get; set; }
    }
}