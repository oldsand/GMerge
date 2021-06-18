namespace GalaxyMerge.Client.Data.Entities
{
    public class ConnectionResource
    {
        public ConnectionResource()
        {
        }
        
        public ConnectionResource(ResourceEntry resource)
        {
            ResourceId = resource.ResourceId;
            Resource = resource;
        }

        public int ResourceId { get; private set; }
        public virtual ResourceEntry Resource { get; private set; }
        public string NodeName { get; set; }
        public string GalaxyName { get; set; }
    }
}