using System.Windows;
using System.Windows.Controls;

namespace GalaxyMerge.Client.Core.Extensions
{
    public class CollapsibleRowDefinition : RowDefinition
    {
        static CollapsibleRowDefinition()
        {
            VisibleProperty = DependencyProperty.Register(nameof(Visible),
                typeof(bool),
                typeof(CollapsibleRowDefinition),
                new PropertyMetadata(true, OnVisibleChanged));
            
            HeightProperty.OverrideMetadata(typeof(CollapsibleColumnDefinition),
                new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null,
                    CoerceHeight));

            MinHeightProperty.OverrideMetadata(typeof(CollapsibleColumnDefinition),
                new FrameworkPropertyMetadata(0d, null,
                    CoerceMinHeight));
        }

        public static readonly DependencyProperty VisibleProperty;

        public bool Visible
        {
            get => (bool)GetValue(VisibleProperty);
            set => SetValue(VisibleProperty, value);
        }

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(HeightProperty);
            d.CoerceValue(MinHeightProperty);
        }

        private static object CoerceHeight(DependencyObject obj, object value)
        {
            return ((CollapsibleRowDefinition)obj).Visible ? value : new GridLength(0);
        }

        private static object CoerceMinHeight(DependencyObject obj, object value)
        {
            return ((CollapsibleRowDefinition)obj).Visible ? value : 0d;
        }
    }
}