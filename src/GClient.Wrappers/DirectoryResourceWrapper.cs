using GClient.Data.Entities;
using GClient.Wrappers.Base;

namespace GClient.Wrappers
{
    public class DirectoryResourceWrapper : ModelWrapper<DirectoryResource>
    {
        public DirectoryResourceWrapper(DirectoryResource model) : base(model)
        {
        }
        
        public string DirectoryName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        
        protected override void Initialize(DirectoryResource model)
        {
            RequireProperty(nameof(DirectoryName));

            base.Initialize(model);
        }
    }
}