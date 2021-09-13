using System;
using System.ComponentModel;

namespace GCommon.Extensions
{
    public static class ObjectExtensions
    {
        public static void Output(this object obj)
        {
            Console.WriteLine(obj.ToString());
        }
        
        public static T ConvertTo<T>(this object value)
        {
            T returnValue;

            if (value is T specifiedType)
                returnValue = specifiedType;
            else
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return converter.IsValid(value) ? 
                    (T)converter.ConvertFrom(value) : default;
            }

            return returnValue;
        }
    }
}