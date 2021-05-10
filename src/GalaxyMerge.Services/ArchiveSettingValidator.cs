using System;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    public class ArchiveSettingValidator
    {
        private readonly string _galaxyName;

        public ArchiveSettingValidator(string galaxyName)
        {
            _galaxyName = galaxyName;
        }
        
        public bool HasValidInclusionOption(int objectId)
        {
            using var objectRepository = new ObjectRepository(_galaxyName);
            var gObject = objectRepository.FindInclude(x => x.ObjectId == objectId, x => x.Template);
            
            using var archiveRepository = new ArchiveRepository(_galaxyName);
            var inclusionSetting = archiveRepository.GetInclusionSetting(gObject.TemplateId);

            if (inclusionSetting.InclusionOption == InclusionOption.None) return false;
            
            if (inclusionSetting.InclusionOption == InclusionOption.All)
                return gObject.IsTemplate || inclusionSetting.IncludesInstances;

            if (inclusionSetting.InclusionOption == InclusionOption.Select)
                return archiveRepository.ObjectExists(gObject.ObjectId);

            throw new InvalidOperationException("Could not determine inclusion option");
        }

        public bool IsValidArchiveTrigger(int operationId)
        {
            using var archiveRepository = new ArchiveRepository(_galaxyName);
            var eventSetting = archiveRepository.GetEventSetting(operationId);

            return eventSetting.IsArchiveTrigger;
        }
    }
}