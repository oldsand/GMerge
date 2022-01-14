using System.Windows.Controls;
using GClient.Core.Naming;
using GClient.Core.Prism;

namespace GClient.Module.Dialogs.Views
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