using System;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class ArchiveEntryData
    {
        public Guid EntryId { get; set; }
        public int ObjectId { get; set; }
        public int Version { get; set; }
        public int? ChangeLogId { get; set; }
        public DateTime ArchivedOn { get; set; }
        public Operation Operation { get; set; }
        public byte[] CompressedData { get; set; }
    }
}