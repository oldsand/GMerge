using System.Windows;

namespace GClient.Resources.Styles
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