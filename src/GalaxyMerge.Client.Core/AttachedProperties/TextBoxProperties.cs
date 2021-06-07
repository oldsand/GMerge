using System.Windows;

namespace GalaxyMerge.Client.Core.AttachedProperties
{
    public static class TextBoxProperties
    {
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.RegisterAttached(
                "DefaultText", 
                typeof(string), 
                typeof(TextBoxProperties), 
                new FrameworkPropertyMetadata(""));
        
        public static string GetDefaultText(DependencyObject obj)
        {
            return (string)obj.GetValue(DefaultTextProperty);
        }
        
        public static void SetDefaultText(DependencyObject obj, string value)
        {
            obj.SetValue(DefaultTextProperty, value);
        }
    }
}