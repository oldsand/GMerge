using System;
using System.Collections.Generic;
using GCommon.Primitives;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Abstractions
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