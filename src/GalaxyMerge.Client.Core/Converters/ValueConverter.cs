using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GalaxyMerge.Client.Core.Converters
{
    public abstract class ValueConverter : MarkupExtension, IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}