using System.ComponentModel.DataAnnotations;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class DirectoryResourceWrapper : ModelWrapper<DirectoryResource>
    {
        public DirectoryResourceWrapper(DirectoryResource model) : base(model, false)
        {
        }

        [Required(ErrorMessage = "Directory name is required")]
        public string DirectoryName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}