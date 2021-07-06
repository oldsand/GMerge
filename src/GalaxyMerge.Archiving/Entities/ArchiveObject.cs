using System;
using System.Collections.Generic;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
{
    public class ArchiveObject
    {
        private readonly List<ArchiveEntry> _entries = new List<ArchiveEntry>();
        
        private ArchiveObject()
        {
        }

        public ArchiveObject(int objectId, string tagName, int version, Template template)
        {
            ObjectId = objectId;
            TagName = tagName;
            Version = version;
            Template = template;
            AddedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }
        
        public int ObjectId { get; private set; }
        public string TagName { get; private set; }
        public int Version { get; private set; }
        public Template Template { get; private set; }
        public bool IsTemplate { get; set; }
        public DateTime AddedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }
        public IEnumerable<ArchiveEntry> Entries => _entries;

        public void UpdateTagName(string tagName)
        {
            TagName = tagName;
            ModifiedOn = DateTime.Now;
        }

        public void UpdateVersion(int version)
        {
            Version = version;
            ModifiedOn = DateTime.Now;
        }
        
        public void AddEntry(byte[] data, int? changeLogId = null)
        {
            var entry = new ArchiveEntry(this, data, changeLogId);
            _entries.Add(entry);
            ModifiedOn = DateTime.Now;
        }
    }
}