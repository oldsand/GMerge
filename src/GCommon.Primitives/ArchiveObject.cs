using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Core.Extensions;
using GCommon.Primitives.Enumerations;
using NLog.Fluent;

namespace GCommon.Primitives
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
            TagName = tagName ?? throw new ArgumentNullException(nameof(tagName), "tagName can not be null");
            Version = version;
            Template = template ?? throw new ArgumentNullException(nameof(template), "template can not be null");
            IsTemplate = tagName.StartsWith("$");
            AddedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
            Logs = new List<ArchiveLog>();
        }

        public int ObjectId { get; }
        public string TagName { get; private set; }
        public int Version { get; private set; }
        public Template Template { get; private set; }
        public bool IsTemplate { get; private set; }
        public DateTime AddedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }
        public IEnumerable<ArchiveEntry> Entries => _entries.AsReadOnly();
        public IEnumerable<ArchiveLog> Logs { get; private set; }

        public ArchiveEntry GetLatestEntry()
        {
            return _entries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault();
        }
        
        public ArchiveLog GetLatestLog()
        {
            return Logs.OrderByDescending(x => x.ChangedOn).FirstOrDefault();
        }

        public void Archive(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "data can not be null");
            if (IsCurrent(data)) return;

            var entry = new ArchiveEntry(this, data);
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