using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Primitives;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfigurationBuilder
    {
        public string FileName { get; private set; }
        public string ConnectionString { get; private set; }
        internal DbContextOptions<ArchiveContext> ContextOptions { get; private set; }
        public GalaxyInfo GalaxyInfo { get; private set; }
        public List<EventSetting> EventSettings { get; private set; }
        public List<InclusionSetting> InclusionSettings { get; private set; }

        private ArchiveConfigurationBuilder()
        {
            EventSettings = new List<EventSetting>();
            InclusionSettings = new List<InclusionSetting>();
            
            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                UpdateOperationSetting(operation, false);
            
            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                UpdateInclusionSetting(template, InclusionOption.None, false);
        }

        public ArchiveConfigurationBuilder(string galaxyName) : this()
        {
            UpdateInfo(new GalaxyInfo(galaxyName));
            UpdateConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName));
        }

        public ArchiveConfigurationBuilder(string galaxyName, int? version, string cdiVersion, string isaVersion) : this()
        {
            UpdateInfo(new GalaxyInfo(galaxyName, version, cdiVersion, isaVersion));
            UpdateConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName));
        }

        public static ArchiveConfigurationBuilder Default(string galaxyName, int? version, string cdiVersion, string isaVersion)
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
        
        public static ArchiveConfigurationBuilder Default(string galaxyName)
        {
            var builder = new ArchiveConfigurationBuilder(galaxyName);
            
            var operations = Enumeration.GetAll<Operation>();
            foreach (var operation in operations)
                builder.UpdateOperationSetting(operation, IsDefaultArchiveOperation(operation));

            var templates = Enumeration.GetAll<Template>();
            foreach (var template in templates)
                builder.UpdateInclusionSetting(template, DefaultInclusionOption(template), DefaultIncludeInstances(template));
            
            return builder;
        }
        
        public ArchiveConfigurationBuilder HasInfo(string galaxyName, int? versionNumber, string cdiVersion, string isaVersion)
        {
            var archiveInfo = new GalaxyInfo(galaxyName, versionNumber, cdiVersion, isaVersion);
            UpdateInfo(archiveInfo);
            return this;
        }

        public ArchiveConfigurationBuilder HasConnectionString(string connectionString)
        {
            UpdateConnectionString(connectionString);
            return this;
        }

        public ArchiveConfigurationBuilder SetTrigger(Operation operation, bool isArchiveTrigger = true)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "Operation value cannot be null");
            
            UpdateOperationSetting(operation, isArchiveTrigger);
            return this;
        }

        public ArchiveConfigurationBuilder SetInclusion(Template template, InclusionOption option = null, bool includeInstances = false)
        {
            if (template == null) throw new ArgumentNullException(nameof(template), "Template value cannot be null");
            
            if (option == null)
                option = InclusionOption.All;
            
            UpdateInclusionSetting(template, option, includeInstances);
            return this;
        }

        private void UpdateInfo(GalaxyInfo info)
        {
            GalaxyInfo = info;
        }
        
        private void UpdateConnectionString(string connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);
            ConnectionString = builder.ConnectionString;
            FileName = builder.DataSource;
            ContextOptions = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
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