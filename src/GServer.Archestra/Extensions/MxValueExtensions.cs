using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using GCommon.Core.Extensions;
using GCommon.Primitives.Enumerations;
using GCommon.Primitives.Structs;

namespace GServer.Archestra.Extensions
{
    public static class MxValueExtensions
    {
        /// <summary>
        /// Gets the current underlying value of the MxValue as the specified type
        /// </summary>
        /// <param name="mxValue">The instance of MxValue</param>
        /// <typeparam name="T">The return type</typeparam>
        /// <returns>The value converted to the specified type</returns>
        /// <remarks>
        /// This method will determine the data type from MxValue class GetDataType method
        /// </remarks>
        public static T GetValue<T>(this MxValue mxValue)
        {
            var mxDataType = mxValue.GetDataType();

            return mxDataType switch
            {
                MxDataType.MxBoolean => mxValue.GetValue<T, bool>(v => v.GetBoolean()),
                MxDataType.MxInteger => mxValue.GetValue<T, int>(v => v.GetInteger()),
                MxDataType.MxFloat => mxValue.GetValue<T, float>(v => v.GetFloat()),
                MxDataType.MxDouble => mxValue.GetValue<T, double>(v => v.GetDouble()),
                MxDataType.MxString => mxValue.GetValue<T, string>(v => v.GetString()),
                MxDataType.MxTime => mxValue.GetValue<T, DateTime>(v =>
                {
                    v.GetTime(out var fileTime);
                    return fileTime.ToDateTime();
                }),
                MxDataType.MxElapsedTime => mxValue.GetValue<T, TimeSpan>(v => v.GetElapsedTime().ToTimeSpan()),
                MxDataType.MxReferenceType => mxValue.GetValue<T, Reference>(v => v.GetMxReference().ToPrimitive()),
                MxDataType.MxStatusType => mxValue.GetValue<T, StatusCategory>(v =>
                    v.GetMxStatus().category.ToPrimitive()),
                MxDataType.MxDataTypeEnum => mxValue.GetValue<T, DataType>(v => v.GetMxDataType().ToPrimitive()),
                MxDataType.MxSecurityClassificationEnum => mxValue.GetValue<T, SecurityClassification>(v =>
                    v.GetMxSecurityClassification().ToPrimitive()),
                MxDataType.MxDataQualityType => mxValue.GetValue<T, Quality>(v => (Quality)v.GetMxDataQuality()),
                MxDataType.MxQualifiedEnum => mxValue.GetValue<T, Enumeration>(v =>
                {
                    v.GetCustomEnum(out var value, out var ordinal, out var primitiveId, out var attributeId);
                    return new Enumeration(value, ordinal, primitiveId, attributeId);
                }),
                MxDataType.MxQualifiedStruct => mxValue.GetValue<T, Blob>(v =>
                {
                    var bytes = Array.Empty<byte>();
                    v.GetCustomStructVB(out var id, ref bytes);
                    return Blob.FromData(bytes, id);
                }),
                MxDataType.MxInternationalizedString => mxValue.GetValue<T, string>(v => v.GetInternationalString(0)),
                MxDataType.MxBigString => mxValue.GetValue<T, string>(v => v.GetString()),
                _ => default
            };
        }
        
        /// <summary>
        /// Sets the underlying value of the MxValue based on the type of the proved new value. 
        /// </summary>
        /// <param name="mxValue">The current MxValue</param>
        /// <param name="newValue">The value to set to MxValue</param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="NotSupportedException">Thrown is the type of T is not handled/supported</exception>
        /// <remarks>
        /// This method support all types of MxDataType except for MxStatus, MxQuality, and Internationalized string.
        /// </remarks>
        public static void SetValue<T>(this MxValue mxValue, T newValue)
        {
            switch (newValue)
            {
                case bool value:
                    mxValue.PutBoolean(value);
                    break;
                case IEnumerable<bool> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutBoolean(val));
                    break;
                case int value:
                    mxValue.PutInteger(value);
                    break;
                case IEnumerable<int> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutInteger(val));
                    break;
                case float value:
                    mxValue.PutFloat(value);
                    break;
                case IEnumerable<float> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutFloat(val));
                    break;
                case double value:
                    mxValue.PutDouble(value);
                    break;
                case IEnumerable<double> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutDouble(val));
                    break;
                case string value:
                    mxValue.PutString(value);
                    break;
                case IEnumerable<string> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutString(val));
                    break;
                case DateTime value:
                    mxValue.PutTime(value.ToVbFileTime());
                    break;
                case IEnumerable<DateTime> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutTime(val.ToVbFileTime()));
                    break;
                case TimeSpan value:
                    mxValue.PutElapsedTime(value.ToVbLargeInteger());
                    break;
                case IEnumerable<TimeSpan> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutElapsedTime(val.ToVbLargeInteger()));
                    break;
                case Reference value:
                    mxValue.PutMxReference(value.ToMx());
                    break;
                case IEnumerable<Reference> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutMxReference(val.ToMx()));
                    break;
                case DataType value:
                    mxValue.PutMxDataType(value.ToMx());
                    break;
                case IEnumerable<DataType> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutMxDataType(val.ToMx()));
                    break;
                case SecurityClassification value:
                    mxValue.PutMxSecurityClassification(value.ToMx());
                    break;
                case IEnumerable<SecurityClassification> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutMxSecurityClassification(val.ToMx()));
                    break;
                case Enumeration value:
                    mxValue.PutCustomEnum(value.Value, value.Ordinal, value.PrimitiveId, value.AttributeId);
                    break;
                case IEnumerable<Enumeration> value:
                    mxValue.SetArray(value,
                        (mx, val) => mx.PutCustomEnum(val.Value, val.Ordinal, val.PrimitiveId, val.AttributeId));
                    break;
                case Blob value:
                    mxValue.PutCustomStructVB(value.Guid, value.Data);
                    break;
                case IEnumerable<Blob> value:
                    mxValue.SetArray(value, (mx, val) => mx.PutCustomStructVB(val.Guid, val.Data));
                    break;
                default:
                    throw new NotSupportedException("The type of this object is not supported by SetValue");
            }
        }

        /// <summary>
        /// Determines whether the current MxValue underlying value is an array
        /// </summary>
        /// <param name="mxValue"></param>
        /// <returns></returns>
        public static bool IsArray(this IMxValue mxValue)
        {
            return mxValue.IsArrayValue();
        }

        private static TReturn GetValue<TReturn, TGetter>(this IMxValue mxValue, Func<IMxValue, TGetter> getter)
        {
            if (!mxValue.IsArrayValue())
                return getter(mxValue).ConvertTo<TReturn>();

            var results = new List<TGetter>();
            mxValue.GetDimensionSize(out var size);

            for (var i = 1; i <= size; i++)
            {
                var item = new MxValueClass();
                mxValue.GetElement(i, item);
                results.Add(getter(item));
            }

            return results.ToArray().ConvertTo<TReturn>();
        }

        private static void SetArray<TValue>(this IMxValue mxValue, IEnumerable<TValue> newValue,
            Action<IMxValue, TValue> setter)
        {
            var array = newValue.ToArray();

            mxValue.Empty();

            for (var i = 0; i < array.Length; i++)
            {
                var item = new MxValueClass();
                setter(item, array[i]);
                mxValue.PutElement(i + 1, item);
            }
        }

        private static bool IsArrayValue(this IMxValue mxValue)
        {
            mxValue.GetDimensionCount(out var dimensions);
            return dimensions > 0;
        }
    }
}