using System;
using System.Runtime.Serialization;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class ArchiveEntryData
    {
        [DataMember] public Guid EntryId { get; set; }
        [DataMember] public int ObjectId { get; set; }
        [DataMember] public int Version { get; set; }
        [DataMember] public int? ChangeLogId { get; set; }
        [DataMember] public DateTime ArchivedOn { get; set; }
        [DataMember] public Operation Operation { get; set; }
        [DataMember] public byte[] CompressedData { get; set; }
    }
}