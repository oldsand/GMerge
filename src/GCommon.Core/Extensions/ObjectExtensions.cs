using System;

namespace GCommon.Core.Extensions
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

            if (value is T valueType)
                returnValue = valueType;
            else
                returnValue = (T)Convert.ChangeType(value, typeof(T));

            return returnValue;
        }
    }
}