using System;
using System.Linq;
using GalaxyMerge.Client.Observables.Base;

namespace GalaxyMerge.Client.Observables.Tests.Model
{
    public class ObservableGenericModel : ObservableModel<GenericModel>
    {
        public ObservableGenericModel(GenericModel model) : base(model)
        {
            SubModel = model.SubModel != null 
                ? new ObservableSubModel(model.SubModel) 
                : throw new ArgumentNullException(nameof(model.SubModel), "SubModel cannot be null");
            
            SubModels = model.SubModels != null 
                ? new ChangeTrackingCollection<ObservableSubModel>(model.SubModels.Select(x => new ObservableSubModel(x)).ToList())
                : throw new ArgumentNullException(nameof(model.SubModels), "SubModels cannot be null");
            
            OtherModels = model.OtherModels != null 
                ? new ChangeTrackingCollection<ObservableSubModel>(model.OtherModels.Select(x => new ObservableSubModel(x)).ToList())
                : throw new ArgumentNullException(nameof(model.OtherModels), "OtherModels cannot be null");

            RegisterTrackingObject(SubModel);
            RegisterTrackingObject(SubModels);
            RegisterTrackingObject(OtherModels);

            //User base class registration which clears and re-adds everything
            RegisterCollection(SubModels, model.SubModels);
            
            //User user defined change handler
            RegisterCollection(OtherModels, (s, e) =>
            {
                if (OtherModels.Count > 0)
                {
                    if (e.OldItems != null)
                        foreach (var subModel in e.OldItems.Cast<ObservableSubModel>())
                            Model.RemoveFromOther(subModel.Model);
                    
                    if (e.NewItems == null) return;
                    
                    foreach (var subModel in e.NewItems.Cast<ObservableSubModel>())
                        Model.AddToOther(subModel.Model);

                    return;
                }
                
                Model.ClearOther();
            });
        }

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

        public ObservableSubModel SubModel { get; private set; }

        public ChangeTrackingCollection<ObservableSubModel> SubModels { get; private set; }
        
        public ChangeTrackingCollection<ObservableSubModel> OtherModels { get; private set; }
    }
}