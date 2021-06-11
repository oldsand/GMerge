namespace GalaxyMerge.Client.Data.Entities
{
    public class DirectoryResource
    {
        private DirectoryResource()
        {
        }

        public DirectoryResource(ResourceEntry resource, string directoryName)
        {
            ResourceId = resource.ResourceId;
            Resource = resource;
            DirectoryName = directoryName;
        }
        public int ResourceId { get; private set; }
        public virtual ResourceEntry Resource { get; private set; }
        public string DirectoryName { get; private set; }
    }
}