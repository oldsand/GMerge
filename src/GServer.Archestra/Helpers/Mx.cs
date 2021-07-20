using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GServer.Archestra.Extensions;
using GServer.Archestra.Internal;

[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests")]
[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests.ExtensionTests")]

namespace GServer.Archestra.Helpers
{
    internal static class Mx
    {
        /// <summary>
        /// Constructs a <c>MxValue</c> class with the specified type. Archestra requires calling Put value to initialize the
        /// MxValueClass. This method makes creating a class with the correct type much easier.
        /// 
        /// Supported types are:
        /// <list type="bullet">
        /// <item>
        /// <description>bool</description>
        /// </item>
        /// <item>
        /// <description>int</description>
        /// </item>
        /// /// <item>
        /// <description>double</description>
        /// </item>
        /// /// <item>
        /// <description>float</description>
        /// </item>
        /// /// <item>
        /// <description>string</description>
        /// </item>
        /// /// <item>
        /// <description>DateTime</description>
        /// </item>
        /// /// <item>
        /// <description>TimeSpan</description>
        /// </item>
        /// /// <item>
        /// <description>Reference</description>
        /// </item>
        /// <item>
        /// <description>DataType</description>
        /// </item>
        /// <item>
        /// <description>SecurityClassification</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">When the specified type is not supported</exception>
        public static MxValue Create<T>()
        {
            var type = typeof(T);
            
            // We need to determine the generic type for IEnumerable<T> if provided. Otherwise just use type of T.
            // Since string implements IEnumerable<char>, we are ignoring  it.
            Type baseType;
            if (type != typeof(string))
                baseType = GetGenericEnumerableType(typeof(T)) ?? typeof(T);
            else
                baseType = type;

            var value = new MxValueClass();

            if (baseType == typeof(bool))
                value.PutBoolean(default);
            else if (baseType == typeof(int))
                value.PutInteger(default);
            else if (baseType == typeof(double))
                value.PutDouble(default);
            else if (baseType == typeof(float))
                value.PutFloat(default);
            else if (baseType == typeof(string))
                value.PutString(default);
            else if (baseType == typeof(DateTime))
                value.PutTime(DateTime.UtcNow.ToVbFileTime());
            else if (baseType == typeof(TimeSpan))
                value.PutElapsedTime(TimeSpan.MinValue.ToVbLargeInteger());
            else if (baseType == typeof(Reference))
                value.PutMxReference(new MxReference());
            else if (baseType == typeof(DataType))
                value.PutMxDataType(MxDataType.MxNoData);
            else if (baseType == typeof(SecurityClassification))
                value.PutMxSecurityClassification(MxSecurityClassification.MxSecurityUndefined);
            else
                throw new NotSupportedException($"The specified type {baseType} is not supported for creation");

            var mxValue = new MxValueClass();
            
            //We want to setup an array if the provided type is an IEnumerable of the determined base type.
            if (IsGenericEnumerableOfBaseType(type, baseType))
                mxValue.PutElement(1, value);
            else
                mxValue = value;

            return mxValue;
        }

        /// <summary>
        /// Creates a MxValue class and initialized with the provided data.
        /// </summary>
        /// <param name="value">The value to set on the MxValue</param>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns></returns>
        public static MxValue Create<T>(T value)
        {
            var mxValue = Create<T>();
            mxValue.SetValue(value);
            return mxValue;
        }

        private static Type GetGenericEnumerableType(Type type)
        {
            return type.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .SingleOrDefault();
        }

        private static bool IsGenericEnumerableOfBaseType(Type type, Type baseType)
        {
            return type.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0]).SingleOrDefault() == baseType;
        }
    }
}