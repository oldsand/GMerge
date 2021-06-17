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

        public string OriginatingMachine
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string OriginatingGalaxy
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string OriginatingVersion
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}