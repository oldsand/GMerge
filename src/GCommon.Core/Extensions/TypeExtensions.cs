using System;
using System.Collections;
using System.Linq;

namespace GCommon.Core.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Determine whether a type is IEnumerable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return type != null && type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }
        
        /// <summary>
        /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
        /// or complex (i.e. custom class with public properties and methods).
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new[] { 
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }
        
        /// <summary>
        /// Determine whether a type implements the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="implemented"></param>
        /// <returns></returns>
        public static bool Implements(this Type type, Type implemented)
        {
            return type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEquatable<>));
        }
    }
}