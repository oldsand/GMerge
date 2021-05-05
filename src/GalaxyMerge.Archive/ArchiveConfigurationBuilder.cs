using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Utilities;
using Microsoft.Data.Sqlite;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfigurationBuilder
    {
        public string FileName { get; private set; }
        public string ConnectionString { get; private set; }
        public GalaxyInfo GalaxyInfo { get; private set; }
        public List<EventSetting> EventSettings { get; private set; }
        public List<InclusionSetting> InclusionSettings { get; private set; }

        public ArchiveConfigurationBuilder(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            EventSettings = new List<EventSetting>();
            InclusionSettings = new List<InclusionSetting>();

            SetInfo(new GalaxyInfo(galaxyName, version, cdiVersion, isaVersion));
            SetConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName));
            
            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                UpdateOperationSetting(operation, false);
            
            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                UpdateInclusionSetting(template, InclusionOption.None, false);
        }

        public static ArchiveConfigurationBuilder Default(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            var builder = new ArchiveConfigurationBuilder(galaxyName, version, cdiVersion, isaVersion);
            
            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                builder.UpdateOperationSetting(operation, IsDefaultArchiveOperation(operation));

            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                builder.UpdateInclusionSetting(template, DefaultInclusionOption(template), DefaultIncludeInstances(template));
            
            return builder;
        }
        
        public ArchiveConfigurationBuilder OverrideInfo(string galaxyName, int versionNumber, string cdiVersion, string isaVersion)
        {
            var archiveInfo = new GalaxyInfo(galaxyName, versionNumber, cdiVersion, isaVersion);
            SetInfo(archiveInfo);
            return this;
        }

        public ArchiveConfigurationBuilder OverrideConnectionString(string connectionString)
        {
            SetConnectionString(connectionString);
            return this;
        }

        public ArchiveConfigurationBuilder TriggerOn(Operation operation)
        {
            UpdateOperationSetting(operation, true);
            return this;
        }

        public ArchiveConfigurationBuilder TriggerOff(Operation operation)
        {
            UpdateOperationSetting(operation, false);
            return this;
        }

        private void SetInfo(GalaxyInfo info)
        {
            GalaxyInfo = info;
        }
        
        private void SetConnectionString(string connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);
            ConnectionString = builder.ConnectionString;
            FileName = builder.DataSource;
        }

        private void UpdateOperationSetting(Operation operation, bool isArchiveTrigger)
        {
            var operationSetting = EventSettings.SingleOrDefault(s => s.Operation == operation);

            if (operationSetting == null)
            {
                var setting = new EventSetting(operation, isArchiveTrigger);
                EventSettings.Add(setting);
                return;
            }
            
            operationSetting.SetArchiveTrigger(isArchiveTrigger);
        }

        private void UpdateInclusionSetting(Template template, InclusionOption inclusionOption, bool includeInstances)
        {
            var inclusionSetting = InclusionSettings.SingleOrDefault(s => s.Template == template);

            if (inclusionSetting == null)
            {
                var setting = new InclusionSetting(template, inclusionOption, includeInstances);
                InclusionSettings.Add(setting);
                return;
            }

            inclusionSetting.SetInclusionOption(inclusionOption);
            inclusionSetting.SetIncludeInstance(includeInstances);
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