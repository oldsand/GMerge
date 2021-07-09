// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System;
using System.Collections.Generic;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Data.Entities
{
    public class GObject
    {
        private GObject()
        {
            Derivations = new List<GObject>();
            ContainedObjects = new List<GObject>();
            AreaObjects = new List<GObject>();
            HostedObjects = new List<GObject>();
            ChangeLogs = new List<ChangeLog>();
        }

        public GObject(int objectId, string tagName, int configVersion)
        {
            
        }
        
        public int ObjectId { get; private set; }
        public int TemplateId { get; private set; }
        public int DerivedFromId { get; private set; }
        public int ContainedById { get; private set; }
        public int AreaId { get; private set; }
        public int HostId { get; private set; }
        public int CheckedInPackageId { get; private set; }
        public int CheckedOutPackageId { get; private set; }
        public int DeployedPackageId { get; private set; }
        public int LastDeployedPackageId { get; private set; }
        public string TagName { get; private set; }
        public string ContainedName { get; private set; }
        public string HierarchicalName { get; private set; }
        public int ConfigVersion { get; private set; }
        public int DeployedVersion { get; private set; }
        public Guid? CheckedOutByUserGuid { get; private set; }
        public bool IsTemplate { get; private set; }
        public bool IsSymbol => Enumeration.FromId<Template>(TemplateId).Equals(Template.Symbol);
        public bool IsHidden { get; private set; }
        public short HostingTreeLevel { get; private set; }
        public bool DeploymentPending { get; private set; }
        public TemplateDefinition TemplateDefinition { get; private set; }
        public GObject DerivedFrom { get; private set; }
        public IEnumerable<GObject> Derivations { get; private set; }
        public GObject Container { get; private set; }
        public IEnumerable<GObject> ContainedObjects { get; private set; }
        public GObject Area { get; private set; }
        public IEnumerable<GObject> AreaObjects { get; private set; }
        public GObject Host { get; private set; }
        internal FolderObjectLink FolderObjectLink { get; private set; }
        public Folder Folder => FolderObjectLink.Folder;
        public IEnumerable<GObject> HostedObjects { get; private set; }
        public IEnumerable<ChangeLog> ChangeLogs { get; private set; }
    }
}