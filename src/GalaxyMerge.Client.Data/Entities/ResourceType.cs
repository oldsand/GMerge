using GalaxyMerge.Core;

namespace GalaxyMerge.Client.Data.Entities
{
    public abstract class ResourceType : Enumeration
    {
        private ResourceType(int id, string name) : base(id, name)
        {
        }

        public static readonly ResourceType Undefined = new UndefinedResourceType();
        public static readonly ResourceType Connection = new ConnectionResourceType();
        public static readonly ResourceType Archive = new ArchiveResourceType();
        public static readonly ResourceType Directory = new DirectoryResourceType();

        //todo maybe revisit this
        /*public static ResourceEntry Create(ResourceEntry entry)
        {
            if (entry.ResourceType == Connection)
            {
                var connection = new ConnectionResource(entry);
                entry.Connection = connection;
            }
                

            return null;
        }*/
        
        public abstract string GetFormattedPath(ResourceEntry entry);
        
        public abstract string GetIconTemplateName();

        private class UndefinedResourceType : ResourceType
        {
            public UndefinedResourceType() : base(0, "Undefined")
            {
            }

            public override string GetFormattedPath(ResourceEntry entry)
            {
                return null;
            }

            public override string GetIconTemplateName()
            {
                return null;
            }
        }
        
        private class ConnectionResourceType : ResourceType
        {
            public ConnectionResourceType() : base(1, "Connection")
            {
            }

            public override string GetFormattedPath(ResourceEntry entry)
            {
                return entry?.Connection != null 
                    ? $"{entry.Connection.NodeName}\\{entry.Connection.GalaxyName}" 
                    : null;
            }

            public override string GetIconTemplateName()
            {
                return "Icon.Filled.Galaxy";
            }
        }
        
        private class ArchiveResourceType : ResourceType
        {
            public ArchiveResourceType() : base(2, "Archive")
            {
            }

            public override string GetFormattedPath(ResourceEntry entry)
            {
                return entry?.Archive?.FileName;
            }

            public override string GetIconTemplateName()
            {
                return "Icon.Filled.Database";
            }
        }
        
        private class DirectoryResourceType : ResourceType
        {
            public DirectoryResourceType() : base(3, "Directory")
            {
            }

            public override string GetFormattedPath(ResourceEntry entry)
            {
                return entry?.Directory?.DirectoryName;
            }

            public override string GetIconTemplateName()
            {
                return "Icon.Filled.Folder";
            }
        }
    }
}