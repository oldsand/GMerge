using System;
using System.Collections.Generic;
using System.Linq;
using GCommon.Primitives;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Repositories
{
    internal class InclusionSettingsRepository : IInclusionSettingsRepository
    {
        private readonly ArchiveContext _context;

        public InclusionSettingsRepository(ArchiveContext context)
        {
            _context = context;
        }

        public bool IsIncluded(ArchiveObject archiveObject)
        {
            if (archiveObject == null) throw new ArgumentNullException(nameof(archiveObject), "archiveObject can not be null");
            
            var setting = _context.InclusionSettings.Single(x => x.Template == archiveObject.Template);

            if (setting.InclusionOption == InclusionOption.None) return false;
            
            if (setting.InclusionOption == InclusionOption.All)
                return archiveObject.IsTemplate || setting.IncludeInstances;

            return setting.InclusionOption == InclusionOption.Select 
                   && _context.Objects.Any(x => x.ObjectId == archiveObject.ObjectId);
        }

        public InclusionSetting Get(Template template)
        {
            if (template == null) throw new ArgumentNullException(nameof(template), "template can not be null");
            
            return _context.InclusionSettings.Single(x => x.Template == template);
        }

        public IEnumerable<InclusionSetting> GetAll()
        {
            return _context.InclusionSettings;
        }

        public IEnumerable<InclusionSetting> FindByOption(InclusionOption option)
        {
            return _context.InclusionSettings.Where(x => x.InclusionOption == option);
        }

        public void Configure(Template template, InclusionOption option, bool includeInstance)
        {
            if (template == null) throw new ArgumentNullException(nameof(template), "template can not be null");
            if (option == null) throw new ArgumentNullException(nameof(option), "option can not be null");
            
            var target = _context.InclusionSettings.Single(x => x.Template == template);
            target.InclusionOption = option;
            target.IncludeInstances = includeInstance;
        }
        
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
