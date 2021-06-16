using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Observables.Base;

namespace GalaxyMerge.Client.Observables
{
    public class ObservableResourceEntry : ObservableModel<ResourceEntry>
    {
        public ObservableResourceEntry(ResourceEntry model) : base(model)
        {
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

        public ResourceType ResourceType
        {
            get => GetValue<ResourceType>();
            set => SetValue(value);
        }
    }
}