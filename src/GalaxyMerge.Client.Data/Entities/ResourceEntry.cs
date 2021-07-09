using System;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace GalaxyMerge.Client.Data.Entities
{
    public class ResourceEntry
    {
        private ResourceEntry()
        {
        }

        public ResourceEntry(string resourceName, ResourceType resourceType, string resourceDescription = null)
        {
            ResourceName = resourceName;
            ResourceType = resourceType;
            ResourceDescription = resourceDescription;
            AddedBy = Environment.UserName;
            AddedOn = DateTime.Now;

            InitializeResource(ResourceType);
        }

        public int ResourceId { get; private set; }

        public string ResourceName { get; set; }

        public string ResourceDescription { get; set; }

        public string ResourcePath { get; }

        public ResourceType ResourceType { get; }

        public DateTime AddedOn { get; }

        public string AddedBy { get; }

        public ConnectionResource Connection { get; private set; }

        public ArchiveResource Archive { get; private set; }

        public DirectoryResource Directory { get; private set; }

        private void InitializeResource(ResourceType resourceType)
        {
            if (resourceType == ResourceType.Connection)
            {
                Connection = new ConnectionResource(this);
            }

            if (resourceType == ResourceType.Archive)
            {
                Archive = new ArchiveResource(this);
            }
            
            if (resourceType == ResourceType.Directory)
            {
                Directory = new DirectoryResource(this);
            }
        }
    }
}