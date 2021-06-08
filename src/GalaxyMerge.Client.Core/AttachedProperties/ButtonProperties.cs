using System.Windows;
using System.Windows.Media;

namespace GalaxyMerge.Client.Core.AttachedProperties
{
    public static class ButtonProperties
    {
        /// <summary>
        /// Corner Radius Property.
        /// Adds the ability to set a corner radius on the button control.  
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached(
                "CornerRadius", typeof(double), typeof(ButtonProperties), new FrameworkPropertyMetadata(0d));
        
        public static double GetCornerRadius(DependencyObject obj)
        {
            return (double)obj.GetValue(CornerRadiusProperty);
        }
        
        public static void SetCornerRadius(DependencyObject obj, double value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
    }
}