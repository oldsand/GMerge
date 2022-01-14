using System.Windows;
using System.Windows.Controls;

namespace TestApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            FormControl.Orientation = Orientation.Horizontal;
        }
    }
}