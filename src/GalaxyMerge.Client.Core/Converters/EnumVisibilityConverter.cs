using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GalaxyMerge.Client.Core.Converters
{
    public class EnumVisibilityConverter : MarkupExtension, IValueConverter
    {
        public string VisibleValue { get; set; }
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(VisibleValue)) return Visibility.Collapsed;
            if (value == null) return Visibility.Collapsed;
            return value.ToString() == VisibleValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}