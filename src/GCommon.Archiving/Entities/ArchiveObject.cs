using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Primitives;

namespace GCommon.Archiving.Entities
{
    public class ArchiveObject
    {
        private readonly List<ArchiveEntry> _entries = new();
        private ArchiveObject()
        {
        }

        public ArchiveObject(int objectId, string tagName, int version, Template template)
        {
            ObjectId = objectId;
            TagName = tagName ?? throw new ArgumentNullException(nameof(tagName), "tagName can not be null");
            Version = version;
            Template = template ?? throw new ArgumentNullException(nameof(template), "template can not be null");
            IsTemplate = tagName.StartsWith("$");
            AddedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
            QueuedItems = new List<QueuedEntry>();
        }
        
        public int ObjectId { get; private set; }
        public string TagName { get; private set; }
        public int Version { get; private set; }
        public Template Template { get; private set; }
        public bool IsTemplate { get; private set; }
        public DateTime AddedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }
        public IEnumerable<ArchiveEntry> Entries => _entries;
        public IEnumerable<QueuedEntry> QueuedItems { get; private set; }

        public bool HasEntries()
        {
            return _entries.Any();
        }

        public ArchiveEntry GetLatestEntry()
        {
            return _entries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault();
        }
        
        public void UpdateTagName(string tagName)
        {
            if (TagName == tagName) return;
            TagName = tagName;
            ModifiedOn = DateTime.Now;
        }

        public void UpdateVersion(int version)
        {
            if (Version == version) return;
            Version = version;
            ModifiedOn = DateTime.Now;
        }
        
        public void AddEntry(byte[] data, int? changeLogId = null)
        {
            var entry = new ArchiveEntry(this, data, changeLogId);
            _entries.Add(entry);
            ModifiedOn = DateTime.Now;
        }
        
        public void AddEntries(IEnumerable<ArchiveEntry> entries)
        {
            _entries.AddRange(entries);
            ModifiedOn = DateTime.Now;
        }
    }
}
