using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Core.Extensions;
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

        public ArchiveEntry GetLatestEntry()
        {
            return _entries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault();
        }

        public void AddEntry(ArchiveEntry entry)
        {
            AddArchiveEntry(entry);
        }

        public void AddEntries(IEnumerable<ArchiveEntry> entries)
        {
            foreach (var entry in entries)
                AddArchiveEntry(entry);
        }

        public void AddEntry(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "data can not be null");
            var entry = new ArchiveEntry(this, data);
            AddArchiveEntry(entry);
        }

        private void AddArchiveEntry(ArchiveEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry), "entry can not be null");

            var data = entry.CompressedData.Decompress();
            if (IsCurrent(data)) return;

            _entries.Add(entry);
            ModifiedOn = DateTime.Now;
        }

        private bool IsCurrent(IEnumerable<byte> data)
        {
            if (!_entries.Any()) return false;

            var latest = _entries.OrderByDescending(x => x.ArchivedOn).First();
            var currentData = latest.CompressedData.Decompress();
            return currentData.SequenceEqual(data);
        }
    }
}