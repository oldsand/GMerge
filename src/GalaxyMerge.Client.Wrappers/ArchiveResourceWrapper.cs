using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;

namespace GalaxyMerge.Client.Wrappers
{
    public class ArchiveResourceWrapper : ModelWrapper<ArchiveResource>
    {
        public ArchiveResourceWrapper(ArchiveResource model) : base(model)
        {
        }
        
        public string FileName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        
        protected override void Initialize(ArchiveResource model)
        {
            RequireProperty(nameof(FileName));

            base.Initialize(model);
        }
    }
}