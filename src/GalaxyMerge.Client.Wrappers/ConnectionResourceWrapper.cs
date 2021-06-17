using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class ConnectionResourceWrapper : ModelWrapper<ConnectionResource>
    {
        public ConnectionResourceWrapper(ConnectionResource model) : base(model)
        {
        }

        public string NodeName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

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