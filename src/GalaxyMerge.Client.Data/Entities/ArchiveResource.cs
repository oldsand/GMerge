namespace GalaxyMerge.Client.Data.Entities
{
    public class ArchiveResource
    {
        private ArchiveResource()
        {
        }

        public ArchiveResource(ResourceEntry resource, string fileName, string machine = null, string galaxy = null,
            string version = null)
        {
            ResourceId = resource.ResourceId;
            Resource = resource;
            FileName = fileName;
            OriginatingMachine = machine;
            OriginatingGalaxy = galaxy;
            OriginatingVersion = version;
        }

        public int ResourceId { get; private set; }
        public virtual ResourceEntry Resource { get; private set; }
        public string FileName { get; private set; }
        public string OriginatingMachine { get; private set; }
        public string OriginatingGalaxy { get; private set; }
        public string OriginatingVersion { get; private set; }
    }
}