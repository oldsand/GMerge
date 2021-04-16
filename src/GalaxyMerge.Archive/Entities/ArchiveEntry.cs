using System;
using GalaxyMerge.Core.Extensions;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveEntry
    {
        protected ArchiveEntry()
        {
        }
        
        public ArchiveEntry(int objectId, string tagName, int version, string baseType, byte[] data)
        {
            EntryId = Guid.NewGuid();
            ObjectId = objectId;
            TagName = tagName;
            Version = version;
            BaseType = baseType;
            Created = DateTime.Now;
            OriginalSize = data.Length;
            CompressedData = data.Compress();
            CompressedSize = CompressedData.Length;
        }

        public Guid EntryId { get; private set; }
        public int ObjectId { get; private set; }
        public string TagName { get; private set; }
        public int Version { get; private set; }
        public string BaseType { get; private set; }
        public DateTime Created { get; private set; }
        public long OriginalSize { get; private set; }
        public long CompressedSize { get; private set; }
        public byte[] CompressedData { get; private set; }
    }
}