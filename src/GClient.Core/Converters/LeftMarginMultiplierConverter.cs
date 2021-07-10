using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GClient.Core.Extensions;

namespace GClient.Core.Converters
{
    public class LeftMarginMultiplierConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TreeViewItem item))
                return new Thickness(0);

            return new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
    
    
    public class TreeItemDepthMultiplierConverter : IValueConverter
    {
        public double Multiplier { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TreeViewItem item))
                return 0;

            return Multiplier * item.GetDepth();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}