using GClient.Data.Entities;
using GClient.Wrappers.Base;
using GCommon.Core.Utilities;

namespace GClient.Wrappers
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
        
        public string GetConnectionString()
        {
            return DbStringBuilder.GalaxyString(NodeName, GalaxyName);
        }
    }
}