using System.ComponentModel.DataAnnotations;
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

        protected override void Initialize(ConnectionResource model)
        {
            RequireProperty(nameof(NodeName));
            RequireProperty(nameof(GalaxyName));
            
            base.Initialize(model);
        }
    }
}