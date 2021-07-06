using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving
{
    public class ArchiveConfiguration : IArchiveConfiguration
    {
        private readonly string _galaxyName;
        private readonly ArchestraVersion _version;
        private readonly List<EventSetting> _eventSettings = new();
        private readonly List<InclusionSetting> _inclusionSettings = new();

        private ArchiveConfiguration(string galaxyName, ArchestraVersion version = null)
        {
            _galaxyName = galaxyName;
            _version = version;

            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                UpdateEventSetting(operation, false);
            
            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                UpdateInclusionSetting(template, InclusionOption.None, false);
        }
        
        public static ArchiveConfiguration Default(string galaxyName, ArchestraVersion version = null)
        {
            var builder = new ArchiveConfiguration(galaxyName, version);
            
            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                builder.UpdateEventSetting(operation, IsDefaultArchiveOperation(operation));

            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                builder.UpdateInclusionSetting(template, DefaultInclusionOption(template), DefaultIncludeInstances(template));
            
            return builder;
        }

        public Archive Build()
        {
            var archiveName = $"{_galaxyName}Archive";
            var archive = new Archive(archiveName, _galaxyName, _version);
            //todo configure archive
            return archive;
        }

        public ArchiveConfiguration HasEvent(Operation operation, bool isArchiveEvent = true)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "Operation value cannot be null");
            
            UpdateEventSetting(operation, isArchiveEvent);
            return this;
        }

        public ArchiveConfiguration HasInclusion(Template template, InclusionOption option = null, bool includeInstances = false)
        {
            if (template == null) throw new ArgumentNullException(nameof(template), "Template value cannot be null");

            option ??= InclusionOption.All;
            UpdateInclusionSetting(template, option, includeInstances);
            
            return this;
        }

        private void UpdateEventSetting(Operation operation, bool isArchiveEvent)
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

        private void UpdateInclusionSetting(Template template, InclusionOption inclusionOption, bool includeInstances)
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
        
        private static bool IsDefaultArchiveOperation(Operation operation)
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