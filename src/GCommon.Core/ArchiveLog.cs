using System;
using GCommon.Core.Enumerations;

namespace GCommon.Core
{
    public class ArchiveLog
    {
        private ArchiveLog()
        {
        }
        
        public ArchiveLog(ArchiveEntry archiveEntry, int changeLogId, DateTime changedOn, Operation operation,
            string comment, string userName)
        {
            EntryId = archiveEntry.EntryId;
            Entry = archiveEntry;
            ChangeLogId = changeLogId;
            ObjectId = archiveEntry.ObjectId;
            ArchiveObject = archiveEntry.ArchiveObject;
            ChangedOn = changedOn;
            Operation = operation;
            Comment = comment;
            UserName = userName;
        }

        public Guid EntryId { get; private set; }
        public ArchiveEntry Entry { get; private set; }
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; private set; }
        public DateTime ChangedOn { get; private set; }
        public Operation Operation { get; private set; }
        public string Comment { get; private set; }
        public string UserName { get; private set; }
    }
}