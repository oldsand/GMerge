using System;
using System.Globalization;
using System.Windows.Media;
using GClient.Core.Converters;
using GClient.Resources.Theming;
using NLog;

namespace GClient.Converters
{
    public class LogLevelToBrushConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not LogLevel logLevel)
                throw new InvalidOperationException("Value must be of type LogLevel");

            if (logLevel == LogLevel.Info)
                return (SolidColorBrush) Theme.GetResource(ThemeResourceKey.PrimaryForeground);
            if (logLevel == LogLevel.Warn)
                return (SolidColorBrush) Theme.GetResource(ThemeResourceKey.ErrorBrush);
            if (logLevel == LogLevel.Error)
                return (SolidColorBrush) Theme.GetResource(ThemeResourceKey.ErrorBrush);

            return new SolidColorBrush(Colors.Black);
        }
    }
}