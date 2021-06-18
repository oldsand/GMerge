using System.ComponentModel.DataAnnotations;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class ConnectionResourceWrapper : ModelWrapper<ConnectionResource>
    {
        public ConnectionResourceWrapper(ConnectionResource model) : base(model, false)
        {
        }

        [Required(ErrorMessage = "Node name is required")]
        public string NodeName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Galaxy name is required")]
        public string GalaxyName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Version
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}