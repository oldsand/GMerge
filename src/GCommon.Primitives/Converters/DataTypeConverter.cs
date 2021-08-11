using System;
using System.ComponentModel;
using System.Globalization;
using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives.Converters
{
    public class DataTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ||
                   sourceType == typeof(int) ||
                   base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value switch
            {
                string _ => DataType.FromName(value.ToString()),
                int _ => DataType.FromValue(int.Parse(value.ToString())),
                _ => base.ConvertFrom(context, culture, value)
            };
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!(value is DataType dataType)) return base.ConvertTo(context, culture, value, destinationType);
            
            return destinationType == typeof(int) ? dataType.Value 
                : destinationType == typeof(string) ? dataType.Name 
                : base.ConvertTo(context, culture, dataType, destinationType);
        }
    }
}