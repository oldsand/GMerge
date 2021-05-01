using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core.Utilities;
using Microsoft.Data.Sqlite;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfiguration
    {
        public string FileName { get; private set; }
        public string ConnectionString { get; private set; }
        public ArchiveInfo ArchiveInfo { get; private set; }
        public List<ArchiveEvent> ArchiveEvents { get; private set; }
        public List<ArchiveTemplate> ArchiveTemplates { get; private set; }

        private ArchiveConfiguration()
        {
            ArchiveEvents = new List<ArchiveEvent>();
            ArchiveTemplates = new List<ArchiveTemplate>();
        }
        
        public static ArchiveConfiguration Empty(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            return new ArchiveConfiguration()
                .SetInfo(new ArchiveInfo(galaxyName, version, cdiVersion, isaVersion))
                .SetConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName));
        }
        
        public static ArchiveConfiguration Default(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            return new ArchiveConfiguration()
                .SetInfo(new ArchiveInfo(galaxyName, version, cdiVersion, isaVersion))
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
            var archiveInfo = new ArchiveInfo(galaxyName, versionNumber, cdiVersion, isaVersion);
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

        private ArchiveConfiguration SetInfo(ArchiveInfo info)
        {
            ArchiveInfo = info;
            return this;
        }

        private ArchiveConfiguration SetArchiveEvent(Operation operation)
        {
            var creationEvent = operation == Operation.CreateInstance || operation == Operation.CreateDerivedTemplate;
            ArchiveEvents.Add(new ArchiveEvent(operation.Id, operation.Name, creationEvent));
            return this;
        }

        private ArchiveConfiguration SetArchiveTemplate(Template template)
        {
            ArchiveTemplates.Add(new ArchiveTemplate(template.Id, template.Name));
            return this;
        }
    }
}