using Prism.Regions;

namespace GalaxyMerge.Client.Core.Prism
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
