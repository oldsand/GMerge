using System.Windows;
using System.Windows.Controls;

namespace GClient.Application.Controls
{
    public partial class ResourceItem : UserControl
    {
        public ResourceItem()
        {
            InitializeComponent();
        }

        public Visibility LaunchButtonVisibility
        {
            get => (Visibility) GetValue(LaunchButtonVisibilityProperty);
            set => SetValue(LaunchButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty LaunchButtonVisibilityProperty =
            DependencyProperty.Register(nameof(LaunchButtonVisibility), typeof(Visibility), typeof(ResourceItem),
                new PropertyMetadata(Visibility.Hidden));
    }
}