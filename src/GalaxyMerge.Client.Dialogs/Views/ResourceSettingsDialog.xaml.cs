using System.Windows.Controls;
using GalaxyMerge.Client.Core.Prism;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.Views
{
    public partial class ResourceSettingsDialog : UserControl
    {
        public ResourceSettingsDialog(IRegionManager regionManager)
        {
            InitializeComponent();
            var scopedRegion = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(this,scopedRegion);
            RegionManagerAware.SetRegionManagerAware(this, scopedRegion);
        }
    }
}