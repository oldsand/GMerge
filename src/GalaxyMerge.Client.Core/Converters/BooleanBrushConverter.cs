using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

#pragma warning disable 649

namespace GalaxyMerge.Client.Core.Converters
{
    public class BooleanBrushConverter : MarkupExtension, IValueConverter
    {
        private BooleanBrushConverter _converter;
        
        public BooleanBrushConverter()
        {
        }

        public BooleanBrushConverter(Brush activeBrush, Brush inactiveBrush)
        {
            ActiveBrush = activeBrush;
            InactiveBrush = inactiveBrush;
        }

        public Brush ActiveBrush { get; set; }
        public Brush InactiveBrush { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? new BooleanBrushConverter(ActiveBrush, InactiveBrush);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var active = false;
            
            if (value is bool b)
                active = b;

            return active ? ActiveBrush : InactiveBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var active = false;
            
            if (value is SolidColorBrush brush)
                active = ActiveBrush == brush;
            
            return active;
        }
    }
}