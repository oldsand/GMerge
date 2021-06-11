namespace GalaxyMerge.Client.Data.Entities
{
    public class ConnectionResource
    {
        private ConnectionResource()
        {
        }
        
        public ConnectionResource(ResourceEntry resource, string nodeName, string galaxyName, string version = null)
        {
            ResourceId = resource.ResourceId;
            Resource = resource;
            NodeName = nodeName;
            GalaxyName = galaxyName;
            Version = version;
        }    
        
        public int ResourceId { get; private set; }
        public virtual ResourceEntry Resource { get; private set; }
        public string NodeName { get; private set; }
        public string GalaxyName { get; private set; }
        public string Version { get; private set; }
    }
}