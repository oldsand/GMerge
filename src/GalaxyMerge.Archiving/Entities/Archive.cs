using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
{
    public class Archive
    {
        private readonly List<ArchiveObject> _objects = new();
        private readonly List<EventSetting> _eventSettings = new();
        private readonly List<InclusionSetting> _inclusionSettings = new();
        private readonly List<IgnoreSetting> _ignoreSettings = new();

        private Archive()
        {
        }

        public Archive(string galaxyName, ArchestraVersion version = null)
        {
            GalaxyName = galaxyName;
            Version = version ?? ArchestraVersion.Sp2012R2P03;
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
        }

        public int ArchiveId { get; private set; }
        public string GalaxyName { get; private set; }
        public ArchestraVersion Version { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        
        public IEnumerable<ArchiveObject> Objects => _objects.AsReadOnly();
        public IEnumerable<EventSetting> EventSettings => _eventSettings.AsReadOnly();
        public IEnumerable<InclusionSetting> InclusionSettings => _inclusionSettings.AsReadOnly();
        public IEnumerable<IgnoreSetting> IgnoreSettings => _ignoreSettings.AsReadOnly();
        
        public bool HasObject(int objectId)
        {
            return _objects.Any(x => x.ObjectId == objectId);
        }
        
        public void UpdateObject(ArchiveObject archiveObject)
        {
            if (_objects.All(x => x.ObjectId != archiveObject.ObjectId))
            {
                _objects.Add(archiveObject);
                return;
            }

            var target = _objects.Single(x => x.ObjectId == archiveObject.ObjectId);
            target.UpdateTagName(archiveObject.TagName);
            target.UpdateVersion(archiveObject.Version);
            target.AddEntries(archiveObject.Entries);
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
            return _eventSettings.Single(x => x.Operation.Id == operationId).IsArchiveEvent;
        }
        
        internal void AddEvent(EventSetting setting)
        {
            _eventSettings.Add(setting);
        }
        
        internal void AddEvents(IEnumerable<EventSetting> eventSettings)
        {
            _eventSettings.AddRange(eventSettings);
        }
        
        internal void AddInclusion(InclusionSetting setting)
        {
            _inclusionSettings.Add(setting);
        }
        
        internal void AddInclusions(IEnumerable<InclusionSetting> settings)
        {
            _inclusionSettings.AddRange(settings);
        }
        
        internal void AddIgnore(IgnoreSetting setting)
        {
            _ignoreSettings.Add(setting);
        }
        
        internal void AddIgnores(IEnumerable<IgnoreSetting> settings)
        {
            _ignoreSettings.AddRange(settings);
        }
    }
}