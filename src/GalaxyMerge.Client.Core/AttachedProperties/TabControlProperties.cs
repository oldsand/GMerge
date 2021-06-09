using System.Windows;
using System.Windows.Media;

namespace GalaxyMerge.Client.Core.AttachedProperties
{
    public static class TabControlProperties
    {
        /// <summary>
        /// Tab Panel Header Background Brush Property.
        /// Adds the ability to set the tab header background color independent of the tab control content background.
        /// </summary>
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.RegisterAttached(
            "HeaderBackground", typeof(SolidColorBrush), typeof(TabControlProperties), new PropertyMetadata(default(SolidColorBrush)));

        public static void SetHeaderBackground(DependencyObject element, SolidColorBrush value)
        {
            element.SetValue(HeaderBackgroundProperty, value);
        }

        public static SolidColorBrush GetHeaderBackground(DependencyObject element)
        {
            return (SolidColorBrush) element.GetValue(HeaderBackgroundProperty);
        }
        
        
        /// <summary>
        /// Tab Panel Header Padding Property.
        /// Adds the ability to set the tab header item padding so to move the start and end of the items in the panel.
        /// </summary>
        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.RegisterAttached(
            "HeaderPadding", typeof(Thickness), typeof(TabControlProperties), new PropertyMetadata(default(Thickness)));

        public static void SetHeaderPadding(DependencyObject element, Thickness value)
        {
            element.SetValue(HeaderPaddingProperty, value);
        }

        public static Thickness GetHeaderPadding(DependencyObject element)
        {
            return (Thickness) element.GetValue(HeaderPaddingProperty);
        }
    }
}