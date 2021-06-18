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

        [Required(ErrorMessage = "Resource type is required")]
        public ResourceType ResourceType => Model.ResourceType;

        public DateTime AddedOn => Model.AddedOn;

        public string AddedBy => Model.AddedBy;

        public ConnectionResourceWrapper Connection { get; private set; }

        public ArchiveResourceWrapper Archive { get; private set; }

        public DirectoryResourceWrapper Directory { get; private set; }

        protected override void Initialize(ResourceEntry model)
        {
            Connection = new ConnectionResourceWrapper(Model.Connection);
            Archive = new ArchiveResourceWrapper(Model.Archive);
            Directory = new DirectoryResourceWrapper(Model.Directory);
            
            base.Initialize(model);
        }
    }
}