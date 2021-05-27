using System.Windows;

namespace GalaxyMerge.Client.Core.Themes.Styles
{
    public partial class DialogStyles
    {
        public DialogStyles()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            window.Close();
        }
    }
}