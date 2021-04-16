using System;

namespace GalaxyMerge.Core.Extensions
{
    public static class TypeExtensions
    {
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