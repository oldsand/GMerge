namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveExclusion
    {
        protected ArchiveExclusion()
        {
        }

        public ArchiveExclusion(string typeName)
        {
            ExclusionName = typeName;
        }

        public int ExclusionId { get; private set; }
        public string ExclusionName { get; private set; }
    }
}