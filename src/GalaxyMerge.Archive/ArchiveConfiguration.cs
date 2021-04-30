using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core.Utilities;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfiguration
    {
        public string ConnectionString { get; private set; }
        public ArchiveInfo ArchiveInfo { get; private set; }
        public List<ArchiveEvent> ArchiveEvents { get; private set; }
        public List<ArchiveExclusion> ArchiveExclusions { get; private set; }

        private ArchiveConfiguration()
        {
            ArchiveEvents = new List<ArchiveEvent>();
            ArchiveExclusions = new List<ArchiveExclusion>();
        }
        
        public static ArchiveConfiguration Default(string galaxyName, int version, string cdiVersion, string isaVersion)
        {
            return new ArchiveConfiguration()
                .HasInfo(new ArchiveInfo(galaxyName, version, cdiVersion, isaVersion))
                .HasConnectionString(ConnectionStringBuilder.BuildArchiveConnection(galaxyName))
                .ArchivesOn(Operation.CheckInSuccess)
                .ArchivesOn(Operation.CreateDerivedTemplate)
                .ArchivesOn(Operation.CreateInstance)
                .ArchivesOn(Operation.Rename)
                .ArchivesOn(Operation.RenameTagName)
                .ExcludesType("$InTouchApp");
        }
        
        public ArchiveConfiguration HasConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }

        public ArchiveConfiguration HasInfo(ArchiveInfo info)
        {
            ArchiveInfo = info;
            return this;
        }

        public ArchiveConfiguration ArchivesOn(Operation operation)
        {
            var creationEvent = operation == Operation.CreateInstance || operation == Operation.CreateDerivedTemplate;
            ArchiveEvents.Add(new ArchiveEvent(operation.Id, operation.Name, creationEvent));
            return this;
        }
        
        public ArchiveConfiguration ExcludesType(string baseType)
        {
            ArchiveExclusions.Add(new ArchiveExclusion(baseType));
            return this;
        }
    }
}