using GCommon.Core.Enumerations;

namespace GCommon.Core
{
    public class InclusionSetting
    {
        private InclusionSetting()
        {
        }

        public InclusionSetting(Template template, InclusionOption inclusionOption = null, bool includeInstances = false)
        {
            Template = template;
            InclusionOption = inclusionOption == null ? InclusionOption.All : inclusionOption;
            IncludeInstances = includeInstances;
        }

        public int InclusionId { get; private set; }
        public int ArchiveId { get; private set; }
        public ArchiveConfig ArchiveConfig { get; private set; }
        public Template Template { get; private set; }
        public InclusionOption InclusionOption { get; set; }
        public bool IncludeInstances { get; set; }
    }
}