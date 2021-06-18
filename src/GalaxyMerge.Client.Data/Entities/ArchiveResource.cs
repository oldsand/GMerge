namespace GalaxyMerge.Client.Data.Entities
{
    public class ArchiveResource
    {
        public ArchiveResource()
        {
        }

        public ArchiveResource(ResourceEntry resource)
        {
            ResourceId = resource.ResourceId;
            Resource = resource;
        }

        public int ResourceId { get; private set; }
        public virtual ResourceEntry Resource { get; private set; }
        public string FileName { get; set; }
    }
}