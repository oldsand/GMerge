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

        public ResourceType ResourceType { get; }

        public DateTime AddedOn { get; }

        public string AddedBy { get; }

        public virtual ConnectionResource Connection { get; private set; }

        public virtual ArchiveResource Archive { get; private set; }

        public virtual DirectoryResource Directory { get; private set; }

        private void InitializeResource(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.None:
                    throw new InvalidOperationException("Cannot initialize resource entry with type set to 'None'");
                case ResourceType.Connection:
                    Connection = new ConnectionResource(this);
                    break;
                case ResourceType.Archive:
                    Archive = new ArchiveResource(this);
                    break;
                case ResourceType.Directory:
                    Directory = new DirectoryResource(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }
    }
}