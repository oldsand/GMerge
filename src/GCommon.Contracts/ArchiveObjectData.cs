using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GCommon.Core;
using GCommon.Core.Enumerations;

namespace GCommon.Contracts
{
    [DataContract(Namespace = "http://www.gmerge.com/Contracts")]
    public class ArchiveObjectData
    {
        [DataMember] public int ObjectId { get; set; }
        [DataMember] public string TagName { get; set; }
        [DataMember] public int Version { get; set; }
        [DataMember] public Template Template { get; set; }
        [DataMember] public DateTime AddedOn { get; set; }
        [DataMember] public DateTime ModifiedOn { get; set; }
        [DataMember] public IEnumerable<ArchiveEntryData> Entries { get; set; }
    }
}