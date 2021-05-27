using System;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Services
{
    public class ArchiveHelper
    {
        private readonly string _galaxyName;

        public ArchiveHelper(string galaxyName)
        {
            _galaxyName = galaxyName;
        }
        
        public bool IsArchivable(QueuedEntry queuedEntry)
        {
            return HasValidInclusionOption(queuedEntry.ObjectId) && IsValidArchiveTrigger(queuedEntry.OperationId);
        }
        
        private bool HasValidInclusionOption(int objectId)
        {
            using var objectRepository = new ObjectRepository(_galaxyName);
            var gObject = objectRepository.FindInclude(x => x.ObjectId == objectId, x => x.Template);
            
            using var archiveRepository = new ArchiveRepository(_galaxyName);
            var inclusionSetting = archiveRepository.GetInclusionSetting(gObject.TemplateId);

            if (inclusionSetting.InclusionOption == InclusionOption.None) return false;
            
            if (inclusionSetting.InclusionOption == InclusionOption.All)
                return gObject.IsTemplate || inclusionSetting.IncludeInstances;

            if (inclusionSetting.InclusionOption == InclusionOption.Select)
                return archiveRepository.ObjectExists(gObject.ObjectId);

            throw new InvalidOperationException("Could not determine inclusion option");
        }

        private bool IsValidArchiveTrigger(int operationId)
        {
            using var archiveRepository = new ArchiveRepository(_galaxyName);
            var eventSetting = archiveRepository.GetEventSetting(operationId);

            return eventSetting.IsArchiveTrigger;
        }
    }
}