using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class InclusionSettingData
    {
        public Template Template { get; set; }
        public InclusionOption InclusionOption { get; set; }
        public bool IncludeInstances { get; set; }
    }
}