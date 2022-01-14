using System;
using System.Globalization;
using System.Windows;

namespace GClient.Core.Converters
{
    public class CornerRadiusAdjuster : ValueConverter
    {
        public CornerRadius Adjustment { get; set; }
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not CornerRadius cornerRadius)
                throw new InvalidOperationException("Value must be of type corner radius");

            cornerRadius.TopLeft += Adjustment.TopLeft;
            cornerRadius.TopRight += Adjustment.TopRight;
            cornerRadius.BottomRight += Adjustment.BottomRight;
            cornerRadius.BottomLeft += Adjustment.BottomLeft;

            return cornerRadius;
        }
    }
}