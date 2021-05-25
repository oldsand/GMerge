using System;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Primitives;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveEntry
    {
        private ArchiveEntry()
        {
        }
        
        public ArchiveEntry(int objectId, int version, byte[] data, int? changeLogId = null)
        {
            ObjectId = objectId;
            Version = version;
            ChangeLogId = changeLogId;
            ArchivedOn = DateTime.Now;
            OriginalSize = data.Length;
            CompressedData = data.Compress();
            CompressedSize = CompressedData.Length;
        }
        
        public ArchiveEntry(ArchiveObject archiveObject, byte[] data, int? changeLogId = null)
        {
            ObjectId = archiveObject.ObjectId;
            Version = archiveObject.Version;
            ChangeLogId = changeLogId;
            ArchiveObject = archiveObject;
            ArchivedOn = DateTime.Now;
            OriginalSize = data.Length;
            CompressedData = data.Compress();
            CompressedSize = CompressedData.Length;
        }

        public Guid EntryId { get; private set; }
        public int ObjectId { get; private set; }
        public ArchiveObject ArchiveObject { get; private set; }
        public int Version { get; private set; }
        public DateTime ArchivedOn { get; private set; }
        public int? ChangeLogId { get; private set; }
        public long OriginalSize { get; private set; }
        public long CompressedSize { get; private set; }
        public byte[] CompressedData { get; private set; }
    }
}