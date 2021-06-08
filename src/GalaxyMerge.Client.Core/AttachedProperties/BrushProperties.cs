using System.Windows;
using System.Windows.Media;

namespace GalaxyMerge.Client.Core.AttachedProperties
{
    public static class BrushProperties
    {
        /// <summary>
        /// Pressed Background Brush Property.
        /// Adds the ability to set the background brush when the control is pressed.   
        /// </summary>
        public static readonly DependencyProperty PressedBackgroundBrushProperty = DependencyProperty.RegisterAttached(
            "PressedBackgroundBrush", typeof(SolidColorBrush), typeof(BrushProperties), new PropertyMetadata(default(SolidColorBrush)));
        
        public static void SetPressedBackgroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(PressedBackgroundBrushProperty, value);
        }
        
        public static SolidColorBrush GetPressedBackgroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush) obj.GetValue(PressedBackgroundBrushProperty);
        }
        

        /// <summary>
        /// Pressed Foreground Brush Property.
        /// Adds the ability to set the foreground brush when the control is pressed.   
        /// </summary>
        public static readonly DependencyProperty PressedForegroundBrushProperty = DependencyProperty.RegisterAttached(
            "PressedForegroundBrush", typeof(SolidColorBrush), typeof(BrushProperties), new PropertyMetadata(default(SolidColorBrush)));

        public static void SetPressedForegroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(PressedForegroundBrushProperty, value);
        }

        public static SolidColorBrush GetPressedForegroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush) obj.GetValue(PressedForegroundBrushProperty);
        }
        

        /// <summary>
        /// Pressed Border Brush Property.
        /// Adds the ability to set the border brush when the control is pressed.   
        /// </summary>
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.RegisterAttached(
            "PressedBorderBrush", typeof(SolidColorBrush), typeof(BrushProperties), new PropertyMetadata(default(SolidColorBrush)));
        
        public static void SetPressedBorderBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(PressedBorderBrushProperty, value);
        }
        
        public static SolidColorBrush GetPressedBorderBrush(DependencyObject obj)
        {
            return (SolidColorBrush) obj.GetValue(PressedBorderBrushProperty);
        }

        
        /// <summary>
        /// Mouse Over Background Brush Property.
        /// Adds the ability to set the background brush when the control is mouse over.   
        /// </summary>
        public static readonly DependencyProperty MouseOverBackgroundBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBackgroundBrush", typeof(SolidColorBrush), typeof(BrushProperties), new PropertyMetadata(default(SolidColorBrush)));
        
        public static void SetMouseOverBackgroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(MouseOverBackgroundBrushProperty, value);
        }
        
        public static SolidColorBrush GetMouseOverBackgroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush) obj.GetValue(MouseOverBackgroundBrushProperty);
        }

        
        /// <summary>
        /// Mouse Over Foreground Brush Property.
        /// Adds the ability to set the foreground brush when the control is mouse over.   
        /// </summary>
        public static readonly DependencyProperty MouseOverForegroundBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverForegroundBrush", typeof(SolidColorBrush), typeof(BrushProperties), new PropertyMetadata(default(SolidColorBrush)));

        public static void SetMouseOverForegroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(MouseOverForegroundBrushProperty, value);
        }

        public static SolidColorBrush GetMouseOverForegroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush) obj.GetValue(MouseOverForegroundBrushProperty);
        }
        
        
        /// <summary>
        /// Mouse Over Border Brush Property.
        /// Adds the ability to set the border brush when the control is mouse over.   
        /// </summary>
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBorderBrush", typeof(SolidColorBrush), typeof(BrushProperties), new PropertyMetadata(default(SolidColorBrush)));
        
        public static void SetMouseOverBorderBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }
        
        public static SolidColorBrush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (SolidColorBrush) obj.GetValue(MouseOverBorderBrushProperty);
        }
    }
}