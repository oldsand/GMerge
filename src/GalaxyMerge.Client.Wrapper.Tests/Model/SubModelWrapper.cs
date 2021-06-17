using System.ComponentModel.DataAnnotations;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrapper.Tests.Model
{
    public class SubModelWrapper : ModelWrapper<SubModel>
    {
        public SubModelWrapper(SubModel model) : base(model)
        {
        }

        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "City is required")]
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