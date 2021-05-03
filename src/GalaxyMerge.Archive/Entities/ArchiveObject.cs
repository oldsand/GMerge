using System;
using System.Collections.Generic;

namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveObject
    {
        protected ArchiveObject()
        {
        }

        public ArchiveObject(int objectId, string tagName, int version, string baseType)
        {
            ObjectId = objectId;
            TagName = tagName;
            Version = version;
            BaseType = baseType;
            AddedOn = DateTime.Now;
            Entries = new List<ArchiveEntry>();
        }
        
        public int ObjectId { get; private set; }
        public string TagName { get; private set; }
        public int Version { get; private set; }
        public string BaseType { get; private set; }
        public DateTime AddedOn { get; private set; }
        public IEnumerable<ArchiveEntry> Entries { get; private set; }
    }
}