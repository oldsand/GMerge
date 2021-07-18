using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using ArchestrA.GRAccess;
using GCommon.Primitives;

namespace GServer.Archestra.Extensions
{
    public static class MxValueExtensions
    {
        public static MxValue AsType<T>(this MxValueClass mxValueClass)
        {
            var type = typeof(T);
            
            if (type == typeof(bool))
                mxValueClass.PutBoolean(default);
            if (type == typeof(int))
                mxValueClass.PutInteger(default);
            if (type == typeof(double))
                mxValueClass.PutDouble(default);
            if (type == typeof(float))
                mxValueClass.PutFloat(default);
            if (type == typeof(string))
                mxValueClass.PutString(default);
            if (type == typeof(DateTime))
                mxValueClass.PutTime(default);

            return mxValueClass;
        }
        
        /// <summary>
        /// Generic method wraps all calls on MxValue to get value of specified type.
        /// This method will attempt to determine the data type from MxValue class, but you can also provide the type
        /// since MxValue data type property is not always set until a value is applied via a Put.
        /// </summary>
        /// <param name="mxValue">The instance of MxValue</param>
        /// <typeparam name="T">The return type</typeparam>
        /// <returns>The value converted to the specified type</returns>
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
                MxDataType.MxReferenceType => mxValue.GetValue<T, string>(v => v.GetMxReference().FullReferenceString),
                MxDataType.MxStatusType => mxValue.GetValue<T, StatusCategory>(v => v.GetMxStatus().category.ToPrimitiveType()),
                MxDataType.MxDataTypeEnum => mxValue.GetValue<T, DataType>(v => v.GetMxDataType().ToPrimitiveType()),
                MxDataType.MxSecurityClassificationEnum => mxValue.GetValue<T, SecurityClassification>(v => v.GetMxSecurityClassification().ToPrimitiveType()),
                MxDataType.MxDataQualityType => mxValue.GetValue<T, Quality>(v => (Quality) v.GetMxDataQuality()),
                MxDataType.MxQualifiedEnum => mxValue.GetValue<T, string>(v =>
                {
                    mxValue.GetCustomEnum(out var value, out _, out _, out _);
                    return value;
                }),
                //MxDataType.MxQualifiedStruct => null,
                MxDataType.MxInternationalizedString => mxValue.GetValue<T, string>(v => v.GetInternationalString(0)),
                MxDataType.MxBigString => mxValue.GetValue<T, string>(v => v.GetString()),
                _ => default
            };
        }

        public static void SetValue<T>(this MxValue mxValue, T newValue, DataType dataType = null)
        {
            var mxDataType = mxValue.GetDataType();
            var type = dataType == null ? mxDataType : dataType.ToMxType();

            switch (type)
            {
                case MxDataType.MxDataTypeUnknown:
                    break;
                case MxDataType.MxNoData:
                    mxValue.Empty();
                    break;
                case MxDataType.MxBoolean:
                    mxValue.SetValue<T, bool>(newValue, (v, x) => v.PutBoolean(x));
                    break;
                case MxDataType.MxInteger:
                    mxValue.SetValue<T, int>(newValue, (v, x) => v.PutInteger(x));
                    break;
                case MxDataType.MxFloat:
                    mxValue.SetValue<T, float>(newValue, (v, x) => v.PutFloat(x));
                    break;
                case MxDataType.MxDouble:
                    mxValue.SetValue<T, double>(newValue, (v, x) => v.PutDouble(x));
                    break;
                case MxDataType.MxString:
                    mxValue.SetValue<T, string>(newValue, (v, x) => v.PutString(x));
                    break;
                case MxDataType.MxTime:
                    mxValue.SetValue<T, DateTime>(newValue, (v, x) => v.PutTime(x.ToVbFileTime()));
                    break;
                case MxDataType.MxElapsedTime:
                    mxValue.SetValue<T, TimeSpan>(newValue, (v, x) => v.PutElapsedTime(x.ToVbLargeInteger()));
                    break;
                case MxDataType.MxReferenceType:
                    //TODO Need to figure out how this can be set especially for the script aliases
                    mxValue.SetReference(newValue);
                    break;
                case MxDataType.MxStatusType:
                    //Status is a runtime system writable attribute. It doesn't make sense to set this type.
                    break;
                case MxDataType.MxDataTypeEnum:
                    mxValue.SetValue<T, DataType>(newValue, (v, x) => v.PutMxDataType(x.ToMxType()));
                    break;
                case MxDataType.MxSecurityClassificationEnum:
                    mxValue.SetValue<T, SecurityClassification>(newValue, (v, x) => v.PutMxSecurityClassification(x.ToMxType()));
                    break;
                case MxDataType.MxDataQualityType:
                    //Quality is a runtime system writable attribute. It doesn't make sense to set this type.
                    break;
                case MxDataType.MxQualifiedEnum:
                    mxValue.SetValue<T, string>(newValue, (v, x) => v.PutCustomEnum(x, 0, 0, 0));
                    break;
                case MxDataType.MxQualifiedStruct:
                    //TODO Need to figure this one out still
                    break;
                case MxDataType.MxInternationalizedString:
                    mxValue.SetValue<T, string>(newValue, (v, x) => v.PutInternationalString(0, x));
                    break;
                case MxDataType.MxBigString:
                    mxValue.SetValue<T, string>(newValue, (v, x) => v.PutString(x));
                    break;
                case MxDataType.MxDataTypeEND:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), "The data type for this object is not defined");
            }
        }
        
        private static TReturn GetValue<TReturn, TGetter>(this IMxValue mxValue, Func<IMxValue, TGetter> getter)
        {
            if (!mxValue.IsArray())
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
        
        private static void SetValue<TValue, TSetter>(this IMxValue mxValue, TValue newValue, Action<IMxValue, TSetter> setter)
        {
            if (!mxValue.IsArray())
            {
                setter(mxValue, newValue.ConvertTo<TSetter>());
                return;
            }

            var array = newValue.ConvertTo<TSetter[]>();
            mxValue.Resize(array.Length);
                
            for (var i = 0; i < array.Length; i++)
            {
                var item = new MxValueClass();
                setter.Invoke(item, array[i]);
                mxValue.PutElement(i + 1, item);
            }
        }
        
        private static T ConvertTo<T>(this object value)
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
        
        private static bool IsArray(this IMxValue mxValue)
        {
            mxValue.GetDimensionCount(out var dimensions);
            return dimensions > 0;
        }
        
        private static void Resize(this IMxValue mxValue, int length)
        {
            mxValue.GetDimensionSize(out var count);
            if (count > length)
                mxValue.Empty();
        }

        private static void SetReference<T>(this IMxValue mxValue, T newValue)
        {
            if (mxValue.GetDataType() != MxDataType.MxReferenceType) return;

            if (!mxValue.IsArray())
            {
                var reference = mxValue.GetMxReference();
                reference.FullReferenceString = newValue.ConvertTo<string>();
                mxValue.PutMxReference(reference);
                return;
            }

            var array = newValue.ConvertTo<string[]>();
            
            var mxReference = new MxValueClass();
            mxValue.GetElement(1, mxReference);
            
            mxValue.Resize(array.Length);
            
            for (var i = 0; i < array.Length; i++)
            {
                var reference = mxReference.GetMxReference();
                reference.FullReferenceString = array[i];
                var item = new MxValueClass();
                item.PutMxReference(reference);
                mxValue.PutElement(i + 1, item);
            }
        }

        private struct CustomStruct
        {
            public byte[] Data { get; set; }
            public int Guid { get; set; }
        }
        
        //TODO this doesn't work yet... I don't know how pointers work
        private static object TryGetCustomStruct(this IMxValue mxValue)
        {
            try
            {
                var ptr = Marshal.AllocHGlobal(100);
                mxValue.GetCustomStruct(out var guid, out var structSize, ptr);
                
                var str = new CustomStruct();
                Marshal.StructureToPtr(str, ptr, true);
                Marshal.FreeHGlobal(ptr);
                return str;
                
                //var data = new List<byte>();
                /*for (var i = 0; i < structSize; i++)
                {
                    var item =  Marshal.ReadByte(ptr, i);
                    data.Add(item);
                }*/
                
                /*var buffer = new byte[255];
                fixed (byte* p = buffer)
                {
                    var ptr = (IntPtr)p;
                    mxValue.GetCustomStruct(out var guid, out var structSize, ptr);
                }
                return buffer;*/
            }
            catch (COMException)
            {
                return null;
            }
        }

    }
}