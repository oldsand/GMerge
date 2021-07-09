using System;
using System.Collections.Generic;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IInclusionSettingsRepository : IDisposable
    {
        bool IsIncluded(ArchiveObject archiveObject);
        InclusionSetting Get(Template template);
        IEnumerable<InclusionSetting> GetAll();
        IEnumerable<InclusionSetting> FindByOption(InclusionOption option);
        void Configure(Template template, InclusionOption option, bool includeInstance);
    }
}