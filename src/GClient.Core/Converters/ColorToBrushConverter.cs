using System;
using System.Globalization;
using System.Windows.Media;

namespace GClient.Core.Converters
{
    public class ColorToBrushConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Color color ? new SolidColorBrush(color) : null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is SolidColorBrush brush ? brush.Color : null;
        }
    }
}