using System.Windows;
using System.Windows.Controls;

namespace GClient.Core.Extensions
{
    public class CollapsibleColumnDefinition : ColumnDefinition
    {
        static CollapsibleColumnDefinition()
        {
            VisibleProperty = DependencyProperty.Register(nameof(Visible),
                typeof(bool),
                typeof(CollapsibleColumnDefinition),
                new PropertyMetadata(true, OnVisibleChanged));
            
            WidthProperty.OverrideMetadata(typeof(CollapsibleColumnDefinition),
                new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null,
                    CoerceWidth));

            MinWidthProperty.OverrideMetadata(typeof(CollapsibleColumnDefinition),
                new FrameworkPropertyMetadata(0d, null,
                    CoerceMinWidth));
        }

        public static readonly DependencyProperty VisibleProperty;

        public bool Visible
        {
            get => (bool)GetValue(VisibleProperty);
            set => SetValue(VisibleProperty, value);
        }

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(WidthProperty);
            d.CoerceValue(MinWidthProperty);
        }

        private static object CoerceWidth(DependencyObject obj, object value)
        {
            return ((CollapsibleColumnDefinition)obj).Visible ? value : new GridLength(0);
        }

        private static object CoerceMinWidth(DependencyObject obj, object value)
        {
            return ((CollapsibleColumnDefinition)obj).Visible ? value : 0d;
        }
    }
}