using System.Windows;
using System.Windows.Controls;

namespace GalaxyMerge.Client.Dialogs.Views
{
    public partial class AddResourceView : UserControl
    {
        public AddResourceView()
        {
            InitializeComponent();
        }

        private void OnResourceSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var index = ResourceTypeListBox.SelectedIndex;

            if (index == -1)
            {
                ResourceTypeSelectionGrid.Visibility = Visibility.Visible;
                GalaxyConnectionGrid.Visibility = Visibility.Collapsed;
                ArchiveDatabaseGrid.Visibility = Visibility.Collapsed;
                GalaxyFileGrid.Visibility = Visibility.Collapsed;
                return;
            }
            
            ResourceTypeSelectionGrid.Visibility = Visibility.Collapsed;

            switch (index)
            {
                case 0:
                    GalaxyConnectionGrid.Visibility = Visibility.Visible;
                    break;
                case 1:
                    ArchiveDatabaseGrid.Visibility = Visibility.Visible;
                    break;
                case 2:
                    GalaxyFileGrid.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}