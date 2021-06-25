using System.Windows.Controls;
using GalaxyMerge.Client.Core.Prism;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.Views
{
    public partial class NewResourceDialog : UserControl
    {
        public NewResourceDialog(IRegionManager regionManager)
        {
            InitializeComponent();
            var scopedRegion = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(this, scopedRegion);
            RegionManagerAware.SetRegionManagerAware(this, scopedRegion);
        }
    }
}