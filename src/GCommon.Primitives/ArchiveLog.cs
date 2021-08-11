using System;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class ArchiveLog
    {
        private ArchiveLog()
        {
        }
        
        public ArchiveLog(int changeLogId, ArchiveObject archiveObject, DateTime changedOn, Operation operation, string comment, string userName)
        {
            ChangeLogId = changeLogId;
            ObjectId = archiveObject.ObjectId;
            ArchiveObject = archiveObject;
            ChangedOn = changedOn;
            Operation = operation;
            Comment = comment;
            UserName = userName;
            State = ArchiveState.New;
        }
        
        public ArchiveLog(int changeLogId, int objectId, DateTime changedOn, Operation operation, string comment, string userName)
        {
            ChangeLogId = changeLogId;
            ObjectId = objectId;
            ChangedOn = changedOn;
            Operation = operation;
            Comment = comment;
            UserName = userName;
            State = ArchiveState.New;
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; private set; }
        public EntryLog EntryLog { get; private set; }
        public ArchiveEntry Entry => EntryLog?.Entry;
        public DateTime ChangedOn { get; private set; }
        public Operation Operation { get; private set; }
        public string Comment { get; private set; }
        public string UserName { get; private set; }
        public ArchiveState State { get; set; }

        public void AssignEntry(ArchiveEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry), "entry can not be null");

            var entryLog = new EntryLog(entry, this);
            EntryLog = entryLog;
        }
    }
}