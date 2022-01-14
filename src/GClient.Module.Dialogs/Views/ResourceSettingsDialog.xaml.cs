using System.Windows.Controls;
using GClient.Core.Prism;
using Prism.Regions;

namespace GClient.Module.Dialogs.Views
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