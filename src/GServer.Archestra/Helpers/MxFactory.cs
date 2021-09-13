using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ArchestrA.GRAccess;
using GCommon.Core.Enumerations;
using GCommon.Core.Structs;
using GServer.Archestra.Extensions;

[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests")]

namespace GServer.Archestra.Helpers
{
    internal static class MxFactory
    {
        /// <summary>
        /// Constructs a <c>MxValue</c> class for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown if the specified type is not one of DataType enums</exception>
        /// <remarks>Archestra requires calling Put{Type} method  to initialize the MxValueClass.
        /// This method simplifies creating an MxValue with the desired type.
        /// IEnumerable types will handle creation of arrays on the MxValue.
        /// Strings are treated as a string and not as an enumeration of characters.
        /// </remarks>
        public static MxValue Create<T>()
        {
            var type = typeof(T);

            // We need to determine the generic type for IEnumerable<T> if provided. Otherwise just use type of T.
            // Since string implements IEnumerable<char>, we are ignoring it.
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
                value.PutMxReference(MxReference.Create(Reference.DefaultReference));
            else if (baseType == typeof(DataType))
                value.PutMxDataType(MxDataType.MxNoData);
            else if (baseType == typeof(SecurityClassification))
                value.PutMxSecurityClassification(MxSecurityClassification.MxSecurityUndefined);
            else if (baseType == typeof(Enumeration))
                value.PutCustomEnum(string.Empty, 0, 0, 0);
            else if (baseType == typeof(Blob))
                value.PutCustomStructVB(0, Array.Empty<byte>());
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