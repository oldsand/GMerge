using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
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
        public InclusionOption InclusionOption { get; set; }
        public bool IncludeInstances { get; set; }
    }
}