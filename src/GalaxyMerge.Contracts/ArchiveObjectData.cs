using System;
using System.Collections.Generic;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class ArchiveObjectData
    {
        public int ObjectId { get; set; }
        public string TagName { get; set; }
        public int Version { get; set; }
        public Template Template { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public IEnumerable<ArchiveEntryData> Entries { get; set; }
    }
}