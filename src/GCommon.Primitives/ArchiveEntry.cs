using System;
using GCommon.Core.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GCommon.Primitives
{
    public class ArchiveEntry
    {
        private ArchiveEntry()
        {
        }

        public ArchiveEntry(ArchiveObject archiveObject, byte[] data)
        {
            ObjectId = archiveObject.ObjectId;
            Version = archiveObject.Version;
            ArchiveObject = archiveObject;
            ArchivedOn = DateTime.Now;
            OriginalSize = data.Length;
            CompressedData = data.Compress();
            CompressedSize = CompressedData.Length;
        }

        public Guid EntryId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; private set; }
        public EntryLog EntryLog { get; private set; }
        public ArchiveLog Log => EntryLog?.Log;
        public int Version { get; private set; }
        public DateTime ArchivedOn { get; private set; }
        public long OriginalSize { get; private set; }
        public long CompressedSize { get; private set; }
        public byte[] CompressedData { get; private set; }
        
        public void AssignLog(ArchiveLog log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log), "log can not be null");

            var entryLog = new EntryLog(this, log);
            EntryLog = entryLog;
        }
    }
}