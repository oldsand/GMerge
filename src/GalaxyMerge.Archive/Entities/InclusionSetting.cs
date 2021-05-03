using GalaxyMerge.Archive.Enum;

namespace GalaxyMerge.Archive.Entities
{
    public class InclusionSetting
    {
        protected InclusionSetting()
        {
        }

        public InclusionSetting(int templateId, string templateName, InclusionOption inclusionOption)
        {
            TemplateId = templateId;
            TemplateName = templateName;
            InclusionOption = inclusionOption;
        }

        public int TemplateId { get; private set; }
        public string TemplateName { get; private set; }
        public InclusionOption InclusionOption { get; private set; }
    }
}