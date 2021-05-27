using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archive.Entities
{
    public class InclusionSetting
    {
        private InclusionSetting()
        {
        }
        
        public InclusionSetting(Template template)
        {
            TemplateId = template.Id;
            InclusionOption = InclusionOption.All;
            IncludesInstances = false;
        }

        public InclusionSetting(Template template, InclusionOption inclusionOption, bool includesInstances)
        {
            TemplateId = template.Id;
            InclusionOption = inclusionOption;
            IncludesInstances = includesInstances;
        }

        public int TemplateId { get; private set; }
        public Template Template => Enumeration.FromId<Template>(TemplateId);
        public InclusionOption InclusionOption { get; private set; }
        public bool IncludesInstances { get; private set; }
        

        public void SetInclusionOption(InclusionOption inclusionOption)
        {
            InclusionOption = inclusionOption;
        }
        
        public void SetIncludeInstance(bool includeInstances)
        {
            IncludesInstances = includeInstances;
        }
    }
}