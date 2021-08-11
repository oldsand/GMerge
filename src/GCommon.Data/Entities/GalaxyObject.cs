using System;
using System.Collections.Generic;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GCommon.Data.Entities
{
    public class GalaxyObject
    {
        public GalaxyObject()
        {
            Derivations = new List<GalaxyObject>();
            ContainedObjects = new List<GalaxyObject>();
            AreaObjects = new List<GalaxyObject>();
            HostedObjects = new List<GalaxyObject>();
            ChangeLogs = new List<ChangeLog>();
        }

        public int ObjectId { get; set; }
        internal int TemplateId { get; private set; }
        public Template Template
        {
            get => Template.FromValue(TemplateId);
            set => TemplateId = value.Value;
        }
        public int DerivedFromId { get; set; }
        public int ContainedById { get; set; }
        public int AreaId { get; set; }
        public int HostId { get; set; }
        public int CheckedInPackageId { get; set; }
        public int CheckedOutPackageId { get; set; }
        public int DeployedPackageId { get; set; }
        public int LastDeployedPackageId { get; set; }
        public string TagName { get; set; }
        public string ContainedName { get; set; }
        public string HierarchicalName { get; set; }
        public int ConfigVersion { get; set; }
        public int DeployedVersion { get; set; }
        public Guid? CheckedOutByUserGuid { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsSymbol => Template.Equals(Template.Symbol);
        public bool IsClientControl => Template.Equals(Template.ClientControl);
        public bool IsHidden { get; set; }
        public short HostingTreeLevel { get; set; }
        public bool DeploymentPending { get; set; }
        public TemplateDefinition TemplateDefinition { get; set; }
        public GalaxyObject DerivedFrom { get; set; }
        public IEnumerable<GalaxyObject> Derivations { get; set; }
        public GalaxyObject Container { get; set; }
        public IEnumerable<GalaxyObject> ContainedObjects { get; set; }
        public GalaxyObject Area { get; set; }
        public IEnumerable<GalaxyObject> AreaObjects { get; set; }
        public GalaxyObject Host { get; set; }
        internal FolderObjectLink FolderObjectLink { get; set; }
        public Folder Folder => FolderObjectLink.Folder;
        public IEnumerable<GalaxyObject> HostedObjects { get; set; }
        public IEnumerable<ChangeLog> ChangeLogs { get; set; }
    }
}