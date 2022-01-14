using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GClient.Core.Converters
{
    public class CompositeCollectionConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var composite = new CompositeCollection();

            foreach (var item in values)
                if (item is IEnumerable enumerable)
                    composite.Add(new CollectionContainer {Collection = enumerable});

            return composite;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}