using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class ResourceEntryWrapper : ModelWrapper<ResourceEntry>
    {
        private readonly List<string> _existingNames;

        public ResourceEntryWrapper(ResourceEntry model) : base(model)
        {
            _existingNames = new List<string>();
        }

        public ResourceEntryWrapper(ResourceEntry model, List<string> existingNames) : base(model)
        {
            _existingNames = existingNames;
        }
        
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

        public string ResourcePath => Model.ResourcePath;

        public ResourceType ResourceType => Model.ResourceType;
        
        public DateTime AddedOn => Model.AddedOn;

        public string AddedBy => Model.AddedBy;

        public ConnectionResourceWrapper Connection { get; private set; }

        public ArchiveResourceWrapper Archive { get; private set; }

        public DirectoryResourceWrapper Directory { get; private set; }

        protected override void Initialize(ResourceEntry model)
        {
            RequireProperty(nameof(ResourceName));
            
            if (model.Connection != null)
                Connection = new ConnectionResourceWrapper(model.Connection);

            if (model.Archive != null)
                Archive = new ArchiveResourceWrapper(model.Archive);
            
            if (model.Directory != null)
                Directory = new DirectoryResourceWrapper(model.Directory);

            base.Initialize(model);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (_existingNames.Select(n => n.ToLower()).Contains(ResourceName.ToLower()))
                yield return new ValidationResult($"'{ResourceName}' is taken. Resource Name must be unique",
                    new[] {nameof(ResourceName)});
        }
    }
}