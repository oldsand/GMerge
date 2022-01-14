using System;
using GCommon.Core.Enumerations;
using GCommon.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GCommon.Core
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
        public ArchiveLog Log { get; private set; }
        public int Version { get; private set; }
        public DateTime ArchivedOn { get; private set; }
        public long OriginalSize { get; private set; }
        public long CompressedSize { get; private set; }
        public byte[] CompressedData { get; private set; }

        public void UpdateLog(int changeLogId, DateTime changed, Operation operation, string comment, string userName)
        {
            if (changed == null) throw new ArgumentNullException(nameof(changed), "changed can not be null");
            if (operation == null) throw new ArgumentNullException(nameof(operation), "operation can not be null");
            if (comment == null) throw new ArgumentNullException(nameof(comment), "comment can not be null");
            if (userName == null) throw new ArgumentNullException(nameof(userName), "userName can not be null");
            
            Log = new ArchiveLog(this, changeLogId, changed, operation, comment, userName);
        }
    }
}