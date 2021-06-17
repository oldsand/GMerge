using System;
using System.ComponentModel.DataAnnotations;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class ResourceEntryWrapper : ModelWrapper<ResourceEntry>
    {
        public ResourceEntryWrapper(ResourceEntry model) : base(model)
        {
        }

        [Required(ErrorMessage = "Resource name is required")]
        public string ResourceName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ResourceDescription
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ResourceType ResourceType => Model.ResourceType;

        public DateTime AddedOn => Model.AddedOn;

        public string AddedBy => Model.AddedBy;

        public ConnectionResourceWrapper Connection
        {
            get => new(Model.Connection);
            set => SetValue<ConnectionResourceWrapper, ConnectionResource>(value);
        }

        public ArchiveResourceWrapper Archive
        {
            get => new(Model.Archive);
            set => SetValue<ArchiveResourceWrapper, ArchiveResource>(value);
        }

        public DirectoryResourceWrapper Directory
        {
            get => new(Model.Directory);
            set => SetValue<DirectoryResourceWrapper, DirectoryResource>(value);
        }

        protected override void Initialize(ResourceEntry model)
        {
            RegisterTrackingObject(nameof(Connection), Connection);
            RegisterTrackingObject(nameof(Archive), Archive);
            RegisterTrackingObject(nameof(Directory), Directory);
        }
    }
}