using System.Windows;
using GClient.Resources.Theming;

namespace GClient.Resources.Styles
{
    public partial class WindowStyles
    {
        public WindowStyles()
        {
            InitializeComponent();
        }
        
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            window.Close();
        }
        
        private void OnMaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            if (window.WindowState == WindowState.Normal)
            {
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowState = WindowState.Normal;
            }
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            window.WindowState = WindowState.Minimized;
        }
        
        private void OnThemeClick(object sender, RoutedEventArgs e)
        {
            var theme = Theme.ThemeType == ThemeType.Light ? ThemeType.Dark : ThemeType.Light;
            Theme.LoadThemeType(theme);
        }
    }
}