using System;
using System.Globalization;
using System.Windows;

namespace GalaxyMerge.Client.Core.Converters
{
    public class StringEmptyOrNullVisibilityConverter : ValueConverter
    {
        public Visibility FalseValue { get; set; }
        public Visibility TrueValue { get; set; }

        public StringEmptyOrNullVisibilityConverter()
        {
            FalseValue = Visibility.Visible;
            TrueValue = Visibility.Collapsed;
        }
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return TrueValue;
                
            if (value is not string s)
                throw new ArgumentException("Value must be of type string");

            return string.IsNullOrEmpty(s) ? TrueValue : FalseValue;
        }
    }
}