using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
{
    public class Archive
    {
        private readonly List<ArchiveObject> _objects = new List<ArchiveObject>();
        private readonly List<EventSetting> _eventSettings = new List<EventSetting>();
        private readonly List<InclusionSetting> _inclusionSettings = new List<InclusionSetting>();
        private readonly List<IgnoreSetting> _ignoreSettings = new List<IgnoreSetting>();
        private readonly List<QueuedEntry> _queue = new List<QueuedEntry>();

        private Archive()
        {
        }

        public Archive(string archiveName, string galaxyName, ArchestraVersion version = null)
        {
            ArchiveName = archiveName;
            GalaxyName = galaxyName;
            Version = version ?? ArchestraVersion.Sp2012R2P03;
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
        }

        public int ArchiveId { get; private set; }
        public string ArchiveName { get; private set; }
        public string GalaxyName { get; private set; }
        public ArchestraVersion Version { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        
        public IEnumerable<ArchiveObject> Objects => _objects.AsReadOnly();
        public IEnumerable<EventSetting> EventSettings => _eventSettings.AsReadOnly();
        public IEnumerable<InclusionSetting> InclusionSettings => _inclusionSettings.AsReadOnly();
        public IEnumerable<IgnoreSetting> IgnoreSettings => _ignoreSettings.AsReadOnly();
        public IEnumerable<QueuedEntry> Queue => _queue.AsReadOnly();

        public void AddEvent(EventSetting eventSetting)
        {
            _eventSettings.Add(eventSetting);
        }

        public bool CanArchive(int objectId, int templateId, bool isTemplate, int operationId)
        {
            return IsIncluded(objectId, templateId, isTemplate) && IsTrigger(operationId);
        }
        
        public bool IsIncluded(int objectId, int templateId, bool isTemplate)
        {
            var inclusionSetting = _inclusionSettings.Single(x => x.Template.Id == templateId);

            if (inclusionSetting.InclusionOption == InclusionOption.None) return false;
            
            if (inclusionSetting.InclusionOption == InclusionOption.All)
                return isTemplate || inclusionSetting.IncludeInstances;

            return inclusionSetting.InclusionOption == InclusionOption.Select 
                   && _objects.Any(x => x.ObjectId == objectId);
        }

        public bool IsTrigger(int operationId)
        {
            return _eventSettings.Single(x => x.OperationId == operationId).IsArchiveEvent;
        }
    }
}