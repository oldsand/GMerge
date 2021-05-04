using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Archive.Entities
{
    public class InclusionSetting
    {
        protected InclusionSetting()
        {
        }
        
        public InclusionSetting(Template template)
        {
            TemplateId = template.Id;
            TemplateName = template.Name;
            InclusionOption = InclusionOption.All;
            IncludedInstances = false;
        }

        public InclusionSetting(Template template, InclusionOption inclusionOption, bool includedInstances)
        {
            TemplateId = template.Id;
            TemplateName = template.Name;
            InclusionOption = inclusionOption;
            IncludedInstances = includedInstances;
        }

        public int InclusionId { get; private set; }
        public int TemplateId { get; private set; }
        public string TemplateName { get; private set; }
        public InclusionOption InclusionOption { get; private set; }
        public bool IncludedInstances { get; private set; }
        public Template Template => Enumeration.FromId<Template>(TemplateId);

        public void SetInclusionOption(InclusionOption inclusionOption)
        {
            InclusionOption = inclusionOption;
        }
        
        public void SetIncludeInstance(bool includeInstances)
        {
            IncludedInstances = includeInstances;
        }
    }
}