using System.Windows;

namespace GClient.Core.AttachedProperties
{
    public static class TextBoxProperties
    {
        /// <summary>
        /// Default text property.
        /// Adds the ability to set a default text for the text box control.
        /// Typically the text is only visible when the control has no input text. 
        /// </summary>
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.RegisterAttached(
                "DefaultText", typeof(string), typeof(TextBoxProperties), new FrameworkPropertyMetadata(""));
        
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