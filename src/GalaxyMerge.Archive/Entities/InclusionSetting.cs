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
            IncludeInstances = false;
        }

        public InclusionSetting(Template template, InclusionOption inclusionOption, bool includeInstances)
        {
            TemplateId = template.Id;
            InclusionOption = inclusionOption;
            IncludeInstances = includeInstances;
        }

        public int TemplateId { get; private set; }
        public Template Template => Enumeration.FromId<Template>(TemplateId);
        public InclusionOption InclusionOption { get; private set; }
        public bool IncludeInstances { get; private set; }
        

        public void SetInclusionOption(InclusionOption inclusionOption)
        {
            InclusionOption = inclusionOption;
        }
        
        public void SetIncludeInstance(bool includeInstances)
        {
            IncludeInstances = includeInstances;
        }
    }
}