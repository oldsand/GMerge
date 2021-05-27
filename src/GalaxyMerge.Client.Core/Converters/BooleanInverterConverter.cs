using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace GalaxyMerge.Client.Core.Converters
{
    public class BooleanInverterConverter : MarkupExtension, IValueConverter  
    {
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)  
        {  
            if (value is bool b) return !b;
            return null;  
        }  
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)  
        {  
            throw new NotImplementedException();  
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }  
}