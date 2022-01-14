using Prism.Regions;

namespace GClient.Core.Prism
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
