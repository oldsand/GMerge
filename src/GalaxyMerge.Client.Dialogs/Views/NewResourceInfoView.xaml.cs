using System.Windows.Controls;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Core.Prism;

namespace GalaxyMerge.Client.Dialogs.Views
{
    [DependentView(typeof(ButtonSaveBackView), RegionName.ButtonRegion)]
    public partial class NewResourceInfoView : UserControl
    {
        public NewResourceInfoView()
        {
            InitializeComponent();
        }
    }
}