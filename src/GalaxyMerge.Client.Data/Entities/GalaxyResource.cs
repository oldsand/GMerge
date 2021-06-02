using System;

namespace GalaxyMerge.Client.Data.Entities
{
    public class GalaxyResource
    {
        private GalaxyResource()
        {
        }

        public GalaxyResource(string name, ResourceType type, string connectionString)
        {
            ResourceName = name;
            ResourceType = type;
            ConnectionString = connectionString;
            AddedOn = DateTime.Now;
        }
        
        public int ResourceId { get; private set; }
        public string ResourceName { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public DateTime AddedOn { get; private set; }
        public string ConnectionString { get; private set; }
    }

    public enum ResourceType
    {
        Connection,
        Archive,
        File
    }
}