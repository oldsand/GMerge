using System;

namespace GCommon.Primitives
{
    public class EntryLog
    {
        private EntryLog()
        {
        }
        
        public EntryLog(ArchiveEntry entry, ArchiveLog log)
        {
            EntryId = entry.EntryId;
            LogId = log.ChangeLogId;
            Entry = entry;
            Log = log;
        }
        
        public Guid EntryId { get; private set; }
        public int LogId { get; private set; }
        public ArchiveEntry Entry { get; private set; }
        public ArchiveLog Log { get; private set; }
    }
}