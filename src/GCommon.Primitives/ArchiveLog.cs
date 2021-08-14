using System;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class ArchiveLog
    {
        private ArchiveLog()
        {
        }
        
        public ArchiveLog(int changeLogId, ArchiveEntry archiveEntry, DateTime changedOn, Operation operation, string comment, string userName)
        {
            ChangeLogId = changeLogId;
            ObjectId = archiveEntry.ObjectId;
            ArchiveObject = archiveEntry.ArchiveObject;
            EntryId = archiveEntry.EntryId;
            Entry = archiveEntry;
            ChangedOn = changedOn;
            Operation = operation;
            Comment = comment;
            UserName = userName;
        }

        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; private set; }
        public Guid EntryId { get; private set; }
        public ArchiveEntry Entry { get; private set; }
        public DateTime ChangedOn { get; private set; }
        public Operation Operation { get; private set; }
        public string Comment { get; private set; }
        public string UserName { get; private set; }
    }
}