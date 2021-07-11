using System;
using GCommon.Primitives;

namespace GCommon.Archiving.Entities
{
    public class ChangeLogInfo
    {
        private ChangeLogInfo()
        {
        }
        
        public ChangeLogInfo(int changeLogId, DateTime changedOn, Operation operation, string comment, string userName)
        {
            ChangeLogId = changeLogId;
            ChangedOn = changedOn;
            Operation = operation;
            Comment = comment;
            UserName = userName;
        }
        
        public int ChangeLogId { get; private set; }
        public Guid EntryId { get; set; }
        public ArchiveEntry Entry { get; set; }
        public DateTime ChangedOn { get; private set; }
        public Operation Operation { get; private set; }
        public string Comment { get; private set; }
        public string UserName { get; private set; }
    }
}