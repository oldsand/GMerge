using System;

namespace GalaxyMerge.Logging
{
    public class ArchiveEventLog
    {
        public int LogId { get; set; }
        public int ObjectId { get; set; }
        public int ChangeLogId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string FormattedMessage { get; set; }
        public string ArchiveState { get; set; }
    }
}