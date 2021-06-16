using GalaxyMerge.Client.Observables.Base;

namespace GalaxyMerge.Client.Observables.Tests.Model
{
    public class ObservableSubModel : ObservableModel<SubModel>
    {
        public ObservableSubModel(SubModel model) : base(model)
        {
        }

        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public float Value
        {
            get => GetValue<float>();
            set => SetValue(value);
        }
    }
}