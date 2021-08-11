using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class Archive
    {
        private readonly List<EventSetting> _eventSettings = new List<EventSetting>();
        private readonly List<InclusionSetting> _inclusionSettings = new List<InclusionSetting>();
        private readonly List<IgnoreSetting> _ignoreSettings = new List<IgnoreSetting>();

        private Archive()
        {
        }

        public Archive(string galaxyName, ArchestraVersion version = null)
        {
            GalaxyName = galaxyName;
            Version = version ?? ArchestraVersion.SystemPlatform2012R2P3;
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
            
            var operations = Operation.List;
            foreach (var operation in operations)
                UpsertEvent(operation, DefaultIsArchiveEvent(operation));
            
            var templates = Template.List;
            foreach (var template in templates)
                UpsertInclusion(template, DefaultInclusionOption(template), DefaultIncludeInstances(template));
        }

        public int ArchiveId { get; private set; }
        public string GalaxyName { get; private set; }
        public ArchestraVersion Version { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        public IEnumerable<EventSetting> EventSettings => _eventSettings.AsReadOnly();
        public IEnumerable<InclusionSetting> InclusionSettings => _inclusionSettings.AsReadOnly();
        public IEnumerable<IgnoreSetting> IgnoreSettings => _ignoreSettings.AsReadOnly();

        public Archive ConfigureEvent(Operation operation, bool isArchiveEvent = true)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "operation can not be null");
            
            UpsertEvent(operation, isArchiveEvent);
            return this;
        }
        
        public Archive ConfigureInclusion(Template template, InclusionOption option = null, bool includeInstances = false)
        {
            if (template == null) throw new ArgumentNullException(nameof(template), "template can not be null");

            option ??= InclusionOption.All;
            UpsertInclusion(template, option, includeInstances);
            
            return this;
        }

        private void UpsertEvent(Operation operation, bool isArchiveEvent)
        {
            var eventSetting = _eventSettings.SingleOrDefault(s => s.Operation == operation);

            if (eventSetting == null)
            {
                var setting = new EventSetting(operation, isArchiveEvent);
                _eventSettings.Add(setting);
                return;
            }
            
            eventSetting.IsArchiveEvent = isArchiveEvent;
        }

        private void UpsertInclusion(Template template, InclusionOption inclusionOption, bool includeInstances)
        {
            var inclusionSetting = _inclusionSettings.SingleOrDefault(s => s.Template == template);

            if (inclusionSetting == null)
            {
                var setting = new InclusionSetting(template, inclusionOption, includeInstances);
                _inclusionSettings.Add(setting);
                return;
            }

            inclusionSetting.InclusionOption = inclusionOption;
            inclusionSetting.IncludeInstances = includeInstances;
        }
        
        private static bool DefaultIsArchiveEvent(Operation operation)
        {
            return operation == Operation.CheckInSuccess ||
                   operation == Operation.CreateDerivedTemplate ||
                   operation == Operation.CreateInstance ||
                   operation == Operation.Rename ||
                   operation == Operation.RenameTagName ||
                   operation == Operation.RenameContainedName ||
                   operation == Operation.Upload;
        }

        private static InclusionOption DefaultInclusionOption(Template template)
        {
            return template == Template.UserDefined || template == Template.Symbol
                ? InclusionOption.All
                : InclusionOption.Select;
        }
        
        private static bool DefaultIncludeInstances(Template template)
        {
            return template == Template.Symbol;
        }
    }
}