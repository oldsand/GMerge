using System;
using System.Linq;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using GalaxyMerge.Primitives.Base;

namespace GalaxyMerge.Archiving
{
    public class ArchiveConfiguration
    {
        private readonly Archive _archive;

        private ArchiveConfiguration(string galaxyName, ArchestraVersion version = null)
        {
            _archive = new Archive(galaxyName, version);

            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                UpdateEventSetting(operation, false);
            
            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                UpdateInclusionSetting(template, InclusionOption.None, false);
        }
        
        public static ArchiveConfiguration Default(string galaxyName, ArchestraVersion version = null)
        {
            var configuration = new ArchiveConfiguration(galaxyName, version);
            
            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                configuration.UpdateEventSetting(operation, DefaultIsArchiveEvent(operation));

            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                configuration.UpdateInclusionSetting(template, DefaultInclusionOption(template), DefaultIncludeInstances(template));
            
            return configuration;
        }

        public Archive GenerateArchive()
        {
            return _archive;
        }

        public ArchiveConfiguration ConfigureEvent(Operation operation, bool isArchiveEvent = true)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "Operation value cannot be null");
            
            UpdateEventSetting(operation, isArchiveEvent);
            return this;
        }

        public ArchiveConfiguration ConfigureInclusion(Template template, InclusionOption option = null, bool includeInstances = false)
        {
            if (template == null) throw new ArgumentNullException(nameof(template), "Template value cannot be null");

            option ??= InclusionOption.All;
            UpdateInclusionSetting(template, option, includeInstances);
            
            return this;
        }

        private void UpdateEventSetting(Operation operation, bool isArchiveEvent)
        {
            var eventSetting = _archive.EventSettings.SingleOrDefault(s => s.Operation == operation);

            if (eventSetting == null)
            {
                var setting = new EventSetting(operation, isArchiveEvent);
                _archive.AddEvent(setting);
                return;
            }
            
            eventSetting.IsArchiveEvent = isArchiveEvent;
        }

        private void UpdateInclusionSetting(Template template, InclusionOption inclusionOption, bool includeInstances)
        {
            var inclusionSetting = _archive.InclusionSettings.SingleOrDefault(s => s.Template == template);

            if (inclusionSetting == null)
            {
                var setting = new InclusionSetting(template, inclusionOption, includeInstances);
                _archive.AddInclusion(setting);
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