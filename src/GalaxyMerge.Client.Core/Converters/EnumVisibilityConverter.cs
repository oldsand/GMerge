using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GalaxyMerge.Client.Core.Converters
{
    public class EnumVisibilityConverter : ValueConverter
    {
        public Visibility FalseValue { get; set; }
        public Visibility TrueValue { get; set; }
        public Visibility DefaultValue { get; set; }

        public EnumVisibilityConverter()
        {
            FalseValue = Visibility.Collapsed;
            TrueValue = Visibility.Visible;
            DefaultValue = Visibility.Collapsed;
        }


        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return DefaultValue;
            
            if (!parameter.ToString().Contains("|"))
                return value.ToString() == parameter.ToString() ? TrueValue : FalseValue;
            
            var tokens = parameter.ToString().Split('|');
            return tokens.Any(token => value.ToString() == token) ? TrueValue : FalseValue;
        }
    }
}