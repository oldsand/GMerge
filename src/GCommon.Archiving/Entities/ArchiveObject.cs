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
        private readonly List<ArchiveLog> _logs = new();

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
            
        }

        public int ObjectId { get; private set; }
        public string TagName { get; private set; }
        public int Version { get; private set; }
        public Template Template { get; private set; }
        public bool IsTemplate { get; private set; }
        public DateTime AddedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }
        public IEnumerable<ArchiveEntry> Entries => _entries.AsReadOnly();
        public IEnumerable<ArchiveLog> Logs => _logs.AsReadOnly();

        public ArchiveEntry GetLatestEntry()
        {
            return _entries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault();
        }

        public void Archive(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "data can not be null");
            if (IsCurrent(data)) return;

            var entry = new ArchiveEntry(this, data);
            _entries.Add(entry);
            
            ModifiedOn = DateTime.Now;
        }

        public void AddLog(int logId, DateTime changedOn, Operation operation, string comment, string userName)
        {
            if (changedOn == null) throw new ArgumentNullException(nameof(changedOn), "changedOn can not be null");
            if (operation == null) throw new ArgumentNullException(nameof(operation), "operation can not be null");
            if (comment == null) throw new ArgumentNullException(nameof(comment), "comment can not be null");
            if (userName == null) throw new ArgumentNullException(nameof(userName), "userName can not be null");

            var log = new ArchiveLog(logId, ObjectId, changedOn, operation, comment, userName);
            _logs.Add(log);
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