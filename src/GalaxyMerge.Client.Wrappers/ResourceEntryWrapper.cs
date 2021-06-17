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

        public override void Initialize(ResourceEntry model)
        {
            if (model.Connection != null)
            {
                Connection = new ConnectionResourceWrapper(model.Connection);
                RegisterTrackingObject(Connection);
            }

            if (model.Archive != null)
            {
                Archive = new ArchiveResourceWrapper(model.Archive);
                RegisterTrackingObject(Archive);
            }

            if (model.Directory != null)
            {
                Directory = new DirectoryResourceWrapper(model.Directory);
                RegisterTrackingObject(Directory);
            }
        }

        [Required(ErrorMessage = "Resource name is required")]
        public string ResourceName
        {
            get => GetValue<string>();
            set => SetValue(value, (m, v) => { m.UpdateName(v); });
        }

        public string ResourceDescription
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ResourceType ResourceType => Model.ResourceType;

        public DateTime AddedOn => Model.AddedOn;

        public string AddedBy => Model.AddedBy;

        public ConnectionResourceWrapper Connection { get; private set; }

        public ArchiveResourceWrapper Archive { get; private set; }

        public DirectoryResourceWrapper Directory { get; private set; }
    }
}