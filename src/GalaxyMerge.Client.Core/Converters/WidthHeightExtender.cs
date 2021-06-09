using System;
using System.Globalization;

namespace GalaxyMerge.Client.Core.Converters
{
    public class WidthHeightExtender : BaseConverter
    {
        public WidthHeightExtender()
        {
            Length = 0;
            Operator = ExtendOperator.Plus;
        }
        public int Length { get; set; }
        public ExtendOperator Operator { get; set; }
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double current)
                throw new ArgumentException(@"Value must be of type double to convert the width or height",
                    nameof(value));

            if (Operator == ExtendOperator.Minus)
                return current - Length;

            return current + Length;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
        public enum ExtendOperator
        {
            Plus,
            Minus
        }
    }
}