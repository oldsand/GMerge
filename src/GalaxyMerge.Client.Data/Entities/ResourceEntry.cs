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
        }

        public int ResourceId { get; private set; }
        public string ResourceName { get; private set; }
        public string ResourceDescription { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public DateTime AddedOn { get; private set; }
        public string AddedBy { get; private set; }
        public virtual ConnectionResource Connection { get; private set; }
        public virtual ArchiveResource Archive { get; private set; }
        public virtual DirectoryResource Directory { get; private set; }

        public void UpdateName(string name)
        {
            ResourceName = name;
        }
        
        public void UpdateDescription(string description)
        {
            ResourceDescription = description;
        }

        public void SetConnection(string nodeName, string galaxyName, string version = null)
        {
            Connection = new ConnectionResource(this, nodeName, galaxyName, version);
            Archive = null;
            Directory = null;
        }
        
        public void SetArchive(string fileName, string machineName = null, string galaxyName = null, string version = null)
        {
            Archive = new ArchiveResource(this, fileName, machineName, galaxyName, version);
        }
        
        public void SetDirectory(string directoryName)
        {
            Directory = new DirectoryResource(this, directoryName);
        }
    }
}