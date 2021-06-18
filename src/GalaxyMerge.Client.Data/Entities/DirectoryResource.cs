namespace GalaxyMerge.Client.Data.Entities
{
    public class DirectoryResource
    {
        public DirectoryResource()
        {
        }

        public DirectoryResource(ResourceEntry resource)
        {
            ResourceId = resource.ResourceId;
            Resource = resource;
        }
        public int ResourceId { get; private set; }
        public virtual ResourceEntry Resource { get; private set; }
        public string DirectoryName { get; set; }
    }
}