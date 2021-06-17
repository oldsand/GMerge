using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrapper.Tests.Model
{
    public class GenericModelWrapper : ModelWrapper<GenericModel>
    {
        public GenericModelWrapper(GenericModel model) : base(model)
        {
        }

        protected override void Initialize(GenericModel model)
        {
            SubModel = model.SubModel != null 
                ? new SubModelWrapper(model.SubModel) 
                : throw new ArgumentNullException(nameof(model.SubModel), "SubModel cannot be null");
            
            SubModels = model.SubModels != null 
                ? new ChangeTrackingCollection<SubModelWrapper>(model.SubModels.Select(x => new SubModelWrapper(x)).ToList())
                : throw new ArgumentNullException(nameof(model.SubModels), "SubModels cannot be null");
            
            OtherModels = model.OtherModels != null 
                ? new ChangeTrackingCollection<SubModelWrapper>(model.OtherModels.Select(x => new SubModelWrapper(x)).ToList())
                : throw new ArgumentNullException(nameof(model.OtherModels), "OtherModels cannot be null");

            RegisterTrackingObject(nameof(SubModel), SubModel);
            RegisterTrackingObject(nameof(SubModels), SubModels);
            RegisterTrackingObject(nameof(SubModels), OtherModels);
            RegisterCollection(SubModels, model.SubModels);
            RegisterCollection(OtherModels, (s, e) =>
            {
                if (OtherModels.Count > 0)
                {
                    if (e.OldItems != null)
                        foreach (var subModel in e.OldItems.Cast<SubModelWrapper>())
                            Model.RemoveFromOther(subModel.Model);
                    
                    if (e.NewItems == null) return;
                    
                    foreach (var subModel in e.NewItems.Cast<SubModelWrapper>())
                        Model.AddToOther(subModel.Model);

                    return;
                }
                
                Model.ClearOther();
            });
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == SomeType.Type2 && Description == "")
                yield return new ValidationResult("SubType2 requires a description yo",
                    new[] {nameof(Type), nameof(Description)});
        }

        [Required(ErrorMessage = "Name is required")]
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        
        public string Description
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Number
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public double Value
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public DateTime DateTime
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public SomeType Type
        {
            get => GetValue<SomeType>();
            set => SetValue(value);
        }

        public SubModelWrapper SubModel { get; private set; }

        public ChangeTrackingCollection<SubModelWrapper> SubModels { get; private set; }
        
        public ChangeTrackingCollection<SubModelWrapper> OtherModels { get; private set; }
    }
}