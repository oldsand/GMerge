using System.Windows;
using System.Windows.Controls;

namespace GalaxyMerge.Client.Dialogs.Controls
{
    public partial class SelectionControl : UserControl
    {
        public SelectionControl()
        {
            InitializeComponent();
        }

        public string SelectionTitle
        {
            get => (string) GetValue(SelectionTitleProperty);
            set => SetValue(SelectionTitleProperty, value);
        }

        public static readonly DependencyProperty SelectionTitleProperty =
            DependencyProperty.Register(nameof(SelectionTitle), typeof(string), typeof(SelectionControl),
                new PropertyMetadata(default(string)));

        public string SelectionDescription
        {
            get => (string) GetValue(SelectionDescriptionProperty);
            set => SetValue(SelectionDescriptionProperty, value);
        }

        public static readonly DependencyProperty SelectionDescriptionProperty =
            DependencyProperty.Register(nameof(SelectionDescription), typeof(string), typeof(SelectionControl),
                new PropertyMetadata(default(string)));

        public ControlTemplate IconTemplate
        {
            get => (ControlTemplate) GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }

        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(nameof(IconTemplate), typeof(ControlTemplate), typeof(SelectionControl),
                new PropertyMetadata(default(ControlTemplate)));
    }
}