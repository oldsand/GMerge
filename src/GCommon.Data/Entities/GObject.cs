using System;
using System.Collections.Generic;
using GCommon.Primitives;
using GCommon.Primitives.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GCommon.Data.Entities
{
    public class GObject
    {
        public GObject()
        {
            Derivations = new List<GObject>();
            ContainedObjects = new List<GObject>();
            AreaObjects = new List<GObject>();
            HostedObjects = new List<GObject>();
            ChangeLogs = new List<ChangeLog>();
        }

        public int ObjectId { get; set; }
        public int TemplateId { get; set; }
        public Template Template => Enumeration.FromId<Template>(TemplateId);
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
        public bool IsSymbol => Enumeration.FromId<Template>(TemplateId).Equals(Template.Symbol);
        public bool IsClientControl => Enumeration.FromId<Template>(TemplateId).Equals(Template.ClientControl);
        public bool IsHidden { get; set; }
        public short HostingTreeLevel { get; set; }
        public bool DeploymentPending { get; set; }
        public TemplateDefinition TemplateDefinition { get; set; }
        public GObject DerivedFrom { get; set; }
        public IEnumerable<GObject> Derivations { get; set; }
        public GObject Container { get; set; }
        public IEnumerable<GObject> ContainedObjects { get; set; }
        public GObject Area { get; set; }
        public IEnumerable<GObject> AreaObjects { get; set; }
        public GObject Host { get; set; }
        internal FolderObjectLink FolderObjectLink { get; set; }
        public Folder Folder => FolderObjectLink.Folder;
        public IEnumerable<GObject> HostedObjects { get; set; }
        public IEnumerable<ChangeLog> ChangeLogs { get; set; }
    }
}