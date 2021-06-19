using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class ResourceEntryWrapper : ModelWrapper<ResourceEntry>
    {
        private readonly List<string> _existingNames;

        public ResourceEntryWrapper(ResourceEntry model) : base(model, true)
        {
            _existingNames = new List<string>();
        }

        public ResourceEntryWrapper(ResourceEntry model, List<string> existingNames) : base(model, true)
        {
            _existingNames = existingNames;
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

        public DirectoryResourceWrapper Directory => new(Model.Directory);

        protected override void Register(ResourceEntry model)
        {
            if (Model.Connection != null)
                Connection = new ConnectionResourceWrapper(Model.Connection);

            if (Model.Archive != null)
                Archive = new ArchiveResourceWrapper(Model.Archive);

            base.Register(model);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (_existingNames.Contains(ResourceName))
                yield return new ValidationResult($"{ResourceName} is taken. Resource Name must be unique.",
                    new[] {nameof(ResourceName)});
        }
    }
}