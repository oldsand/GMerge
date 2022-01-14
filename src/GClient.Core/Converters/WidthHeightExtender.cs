using System;
using System.Globalization;

namespace GClient.Core.Converters
{
    public class WidthHeightExtender : ValueConverter
    {
        public WidthHeightExtender()
        {
            Length = 0;
            Operator = Operator.Plus;
        }
        public int Length { get; set; }
        public Operator Operator { get; set; }
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double current)
                throw new ArgumentException(@"Value must be of type double to convert the width or height",
                    nameof(value));

            if (Operator == Operator.Minus)
                return current - Length;

            return current + Length;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}