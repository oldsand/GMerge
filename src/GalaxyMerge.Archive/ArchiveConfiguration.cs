using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfiguration
    {
        public string GalaxyName { get; private set; }
        public string FileName { get; private set; }
        public string ConnectionString { get; private set; }
        internal DbContextOptions<ArchiveContext> ContextOptions { get; private set; }
        public List<EventSetting> EventSettings { get; private set; }
        public List<InclusionSetting> InclusionSettings { get; private set; }

        public static ArchiveConfiguration Load(string galaxyName)
        {
            var connectionString = DbStringBuilder.BuildArchive(galaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            
            using var context = new ArchiveContext(options);

            return new ArchiveConfiguration
            {
                GalaxyName = context.GalaxyInfo.Single().GalaxyName,
                ConnectionString = connectionString,
                ContextOptions = options,
                EventSettings = context.EventSettings.ToList(),
                InclusionSettings = context.InclusionSettings.ToList()
            };
        }
        
        public bool HasValidInclusionOption(int objectId, int templateId, bool isTemplate)
        {
            var inclusionSetting = InclusionSettings.Single(x => x.TemplateId == templateId);

            if (inclusionSetting.InclusionOption == InclusionOption.None) return false;
            
            if (inclusionSetting.InclusionOption == InclusionOption.All)
                return isTemplate || inclusionSetting.IncludesInstances;

            if (inclusionSetting.InclusionOption != InclusionOption.Select) return false;
            
            using var context = new ArchiveContext(ContextOptions);
            return context.ArchiveObjects.Any(x => x.ObjectId == objectId);
        }

        public bool IsValidArchiveTrigger(int operationId)
        {
            return EventSettings.Single(x => x.OperationId == operationId).IsArchiveTrigger;
        }
    }
}