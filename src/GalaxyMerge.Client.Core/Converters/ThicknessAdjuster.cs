using System;
using System.Globalization;
using System.Windows;

namespace GalaxyMerge.Client.Core.Converters
{
    public class ThicknessAdjuster : ValueConverter
    {
        public Thickness Adjustment { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Thickness current)
                throw new ArgumentException("Value must be convertable to a Thickness type");

            current.Left += Adjustment.Left;
            current.Top += Adjustment.Top;
            current.Right += Adjustment.Right;
            current.Bottom += Adjustment.Bottom;

            return current;
        }
    }
}