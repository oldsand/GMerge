using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Enum;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Utilities;
using Microsoft.Data.Sqlite;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfiguration
    {
        public string FileName { get; private set; }
        public string ConnectionString { get; private set; }
        public GalaxyInfo GalaxyInfo { get; private set; }
        public List<EventSetting> ArchiveEvents { get; private set; }
        public List<InclusionSetting> ArchiveTemplates { get; private set; }

        private ArchiveConfiguration()
        {
            ArchiveEvents = new List<EventSetting>();
            ArchiveTemplates = new List<InclusionSetting>();
        }
        
        public static ArchiveConfiguration Empty(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            return new ArchiveConfiguration()
                .SetInfo(new GalaxyInfo(galaxyName, version, cdiVersion, isaVersion))
                .SetConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName));
        }
        
        public static ArchiveConfiguration Default(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            return new ArchiveConfiguration()
                .SetInfo(new GalaxyInfo(galaxyName, version, cdiVersion, isaVersion))
                .SetConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName))
                .SetArchiveEvent(Operation.CheckInSuccess)
                .SetArchiveEvent(Operation.CreateDerivedTemplate)
                .SetArchiveEvent(Operation.CreateInstance)
                .SetArchiveEvent(Operation.Rename)
                .SetArchiveEvent(Operation.RenameTagName)
                .SetArchiveEvent(Operation.RenameContainedName)
                .SetArchiveEvent(Operation.Upload)
                .SetArchiveTemplate(Template.UserDefined)
                .SetArchiveTemplate(Template.Symbol)
                .SetArchiveTemplate(Template.Area)
                .SetArchiveTemplate(Template.AppEngine)
                .SetArchiveTemplate(Template.ViewEngine)
                .SetArchiveTemplate(Template.OpcClient)
                .SetArchiveTemplate(Template.DdeSuiteLinkClient);
        }

        public ArchiveConfiguration WithInfo(string galaxyName, int versionNumber, string cdiVersion, string isaVersion)
        {
            var archiveInfo = new GalaxyInfo(galaxyName, versionNumber, cdiVersion, isaVersion);
            return SetInfo(archiveInfo);
        }

        public ArchiveConfiguration HasConnectionString(string connectionString)
        {
            return SetConnectionString(connectionString);
        }

        public ArchiveConfiguration TriggersWhen(Operation operation)
        {
            return SetArchiveEvent(operation);
        }

        public ArchiveConfiguration AllowType(Template template)
        {
            return SetArchiveTemplate(template);
        }

        private ArchiveConfiguration SetConnectionString(string connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);
            ConnectionString = builder.ConnectionString;
            FileName = builder.DataSource;
            return this;
        }

        private ArchiveConfiguration SetInfo(GalaxyInfo info)
        {
            GalaxyInfo = info;
            return this;
        }

        private ArchiveConfiguration SetArchiveEvent(Operation operation)
        {
            ArchiveEvents.Add(new EventSetting(operation.Id, operation.Name, EventType.FromOperation(operation)));
            return this;
        }

        private ArchiveConfiguration SetArchiveTemplate(Enumeration template)
        {
            var option = DetermineInclusionOption(template);
            ArchiveTemplates.Add(new InclusionSetting(template.Id, template.Name, option));
            return this;
        }

        private static InclusionOption DetermineInclusionOption(Enumeration template)
        {
            return template == Template.UserDefined || template == Template.Symbol
                ? InclusionOption.All
                : InclusionOption.Select;
        }
    }
}