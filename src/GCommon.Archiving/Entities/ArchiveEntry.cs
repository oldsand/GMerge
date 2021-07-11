using System;
using GCommon.Core.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GCommon.Archiving.Entities
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
        public ChangeLogInfo ChangeLog { get; set; }
        public int Version { get; private set; }
        public DateTime ArchivedOn { get; private set; }
        public long OriginalSize { get; private set; }
        public long CompressedSize { get; private set; }
        public byte[] CompressedData { get; private set; }
    }
}