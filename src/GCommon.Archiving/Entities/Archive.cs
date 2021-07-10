using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Primitives;

namespace GCommon.Archiving.Entities
{
    public class Archive
    {
        private readonly List<EventSetting> _eventSettings = new();
        private readonly List<InclusionSetting> _inclusionSettings = new();
        private readonly List<IgnoreSetting> _ignoreSettings = new();

        private Archive()
        {
        }

        public Archive(string galaxyName, ArchestraVersion version = null)
        {
            GalaxyName = galaxyName;
            Version = version ?? ArchestraVersion.SystemPlatform2012R2P3;
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
        }

        public int ArchiveId { get; private set; }
        public string GalaxyName { get; private set; }
        public ArchestraVersion Version { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        
        public IEnumerable<EventSetting> EventSettings => _eventSettings.AsReadOnly();
        public IEnumerable<InclusionSetting> InclusionSettings => _inclusionSettings.AsReadOnly();
        public IEnumerable<IgnoreSetting> IgnoreSettings => _ignoreSettings.AsReadOnly();

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