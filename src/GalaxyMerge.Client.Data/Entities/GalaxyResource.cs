using System;
using Microsoft.Data.SqlClient;

namespace GalaxyMerge.Client.Data.Entities
{
    public class GalaxyResource
    {
        private GalaxyResource()
        {
        }

        public GalaxyResource(string resourceName, ResourceType resourceType, string nodeName = null, string galaxyName = null, string fileName = null)
        {
            ResourceName = resourceName;
            ResourceType = resourceType;
            NodeName = nodeName;
            GalaxyName = galaxyName;
            FileName = fileName;
            AddedOn = DateTime.Now;
        }

        public static GalaxyResource Connection(string resourceName, string nodeName, string galaxyName)
        {
            return new GalaxyResource
            {
                ResourceName = resourceName,
                ResourceType = ResourceType.Connection,
                NodeName = nodeName,
                GalaxyName = galaxyName,
                AddedOn = DateTime.Now
            };
        }

        public GalaxyResource Archive(string resourceName, string fileName)
        {
            return new GalaxyResource
            {
                ResourceName = resourceName,
                ResourceType = ResourceType.Archive,
                FileName = fileName,
                AddedOn = DateTime.Now
            };
        }

        public GalaxyResource File(string resourceName, string fileName)
        {
            return new GalaxyResource
            {
                ResourceName = resourceName,
                ResourceType = ResourceType.File,
                FileName = fileName,
                AddedOn = DateTime.Now
            };
        }

        public int ResourceId { get; private set; }
        public string ResourceName { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public string NodeName { get; private set; }
        public string GalaxyName { get; private set; }
        public string FileName { get; private set; }
        public DateTime AddedOn { get; private set; }
    }

    public enum ResourceType
    {
        Connection,
        Archive,
        File
    }
}