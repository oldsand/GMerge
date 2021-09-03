using System;
using Ardalis.SmartEnum;
using GCommon.Core.Extensions;
using GCommon.Primitives.Structs;

namespace GCommon.Primitives.Enumerations
{
    public abstract class DataType : SmartEnum<DataType>
    {
        private DataType(string name, int value) : base(name, value)
        {
        }

        public static readonly DataType Unknown = new UnknownInternal();
        public static readonly DataType NoData = new NoDataInternal();
        public static readonly DataType Boolean = new BooleanInternal();
        public static readonly DataType Integer = new IntegerInternal();
        public static readonly DataType Float = new FloatInternal();
        public static readonly DataType Double = new DoubleInternal();
        public static readonly DataType String = new StringInternal();
        public static readonly DataType Time = new TimeInternal();
        public static readonly DataType ElapsedTime = new ElapsedTimeInternal();
        public static readonly DataType ReferenceType = new ReferenceTypeInternal();
        public static readonly DataType Status = new StatusTypeInternal();
        public static readonly DataType DataTypeEnum = new DataTypeEnumInternal();
        public static readonly DataType SecurityClassificationEnum = new SecurityClassificationEnumInternal();
        public static readonly DataType DataQuality = new DataQualityInternal();
        public static readonly DataType QualifiedEnum = new QualifiedEnumInternal();
        public static readonly DataType QualifiedStruct = new QualifiedStructInternal();
        public static readonly DataType InternationalizedString = new InternationalizedStringInternal();
        public static readonly DataType BigString = new BigStringInternal();

        public virtual object DefaultValue => default;
        public abstract Type GetClrType();
        public abstract object Parse(string value);
        public abstract object Parse(string[] value);

        private class UnknownInternal : DataType
        {
            public UnknownInternal() : base( "Unknown", -1)
            {
            }

            public override Type GetClrType()
            {
                return null;
            }

            public override object Parse(string value)
            {
                return null;
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }

        private class NoDataInternal : DataType
        {
            public NoDataInternal() : base("No Data", 0)
            {
            }

            public override Type GetClrType()
            {
                return null;
            }

            public override object Parse(string value)
            {
                return null;
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }

        private class BooleanInternal : DataType
        {
            public BooleanInternal() : base("Boolean", 1)
            {
            }
            
            public override object DefaultValue => default(bool);

            public override Type GetClrType()
            {
                return typeof(bool);
            }

            public override object Parse(string value)
            {
                return value.ConvertTo<bool>();
            }

            public override object Parse(string[] value)
            {
                return value.ConvertTo<bool[]>();
            }
        }

        private class IntegerInternal : DataType
        {
            public IntegerInternal() : base("Integer", 2)
            {
            }
            
            public override object DefaultValue => default(int);

            public override Type GetClrType()
            {
                return typeof(int);
            }

            public override object Parse(string value)
            {
                return value.ConvertTo<int>();
            }

            public override object Parse(string[] value)
            {
                return value.ConvertTo<int[]>();
            }
        }

        private class FloatInternal : DataType
        {
            public FloatInternal() : base("Float", 3)
            {
            }

            public override object DefaultValue => default(float);
            
            public override Type GetClrType()
            {
                return typeof(float);
            }

            public override object Parse(string value)
            {
                return value.ConvertTo<float>();
            }

            public override object Parse(string[] value)
            {
                return value.ConvertTo<float[]>();
            }
        }

        private class DoubleInternal : DataType
        {
            public DoubleInternal() : base("Double", 4)
            {
            }

            public override object DefaultValue => default(double);
            
            public override Type GetClrType()
            {
                return typeof(double);
            }
            
            public override object Parse(string value)
            {
                return value.ConvertTo<double>();
            }

            public override object Parse(string[] value)
            {
                return value.ConvertTo<double[]>();
            }
        }

        private class StringInternal : DataType
        {
            public StringInternal() : base("String", 5)
            {
            }
            
            public override object DefaultValue => default(string);
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
            
            public override object Parse(string value)
            {
                return value;
            }

            public override object Parse(string[] value)
            {
                return value;
            }
        }

        private class TimeInternal : DataType
        {
            public TimeInternal() : base("Time", 6)
            {
            }
            
            public override object DefaultValue => default(DateTime);
            
            public override Type GetClrType()
            {
                return typeof(DateTime);
            }
            
            public override object Parse(string value)
            {
                return value.ConvertTo<DateTime>();
            }

            public override object Parse(string[] value)
            {
                return value.ConvertTo<DateTime[]>();
            }
        }

        private class ElapsedTimeInternal : DataType
        {
            public ElapsedTimeInternal() : base("ElapsedTime", 7)
            {
            }
            
            public override object DefaultValue => default(TimeSpan);
            
            public override Type GetClrType()
            {
                return typeof(TimeSpan);
            }
            
            public override object Parse(string value)
            {
                return value.ConvertTo<TimeSpan>();
            }

            public override object Parse(string[] value)
            {
                return value.ConvertTo<TimeSpan[]>();
            }
        }

        private class ReferenceTypeInternal : DataType
        {
            public ReferenceTypeInternal() : base("ReferenceType", 8)
            {
            }

            public override Type GetClrType()
            {
                return typeof(Reference);
            }

            public override object Parse(string value)
            {
                return Reference.FromName(value);
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }
        
        private class StatusTypeInternal : DataType
        {
            public StatusTypeInternal() : base("Status", 9)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(StatusCategory);
            }

            public override object Parse(string value)
            {
                return StatusCategory.FromName(value);
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }

        private class DataTypeEnumInternal : DataType
        {
            public DataTypeEnumInternal() : base( "DataTypeEnum", 10)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(DataType);
            }

            public override object Parse(string value)
            {
                return FromName(value);
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }
        
        private class SecurityClassificationEnumInternal : DataType
        {
            public SecurityClassificationEnumInternal() : base( "SecurityClassificationEnum", 11)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(SecurityClassification);
            }

            public override object Parse(string value)
            {
                return SecurityClassification.FromName(value);
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }
        
        private class DataQualityInternal : DataType
        {
            public DataQualityInternal() : base( "DataQuality", 12)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(Quality);
            }

            public override object Parse(string value)
            {
                return Quality.FromName(value);
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }

        private class QualifiedEnumInternal : DataType
        {
            public QualifiedEnumInternal() : base( "QualifiedEnum", 13)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
            
            public override object Parse(string value)
            {
                return value;
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }
        
        private class QualifiedStructInternal : DataType
        {
            public QualifiedStructInternal() : base( "QualifiedStruct", 14)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(byte[]);
            }
            
            public override object Parse(string value)
            {
                return value;
            }

            public override object Parse(string[] value)
            {
                return null;
            }
        }

        private class InternationalizedStringInternal : DataType
        {
            public InternationalizedStringInternal() : base( "InternationalizedStruct", 15)
            {
            }
            
            public override object DefaultValue => default(string);
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
            
            public override object Parse(string value)
            {
                return value;
            }

            public override object Parse(string[] value)
            {
                return value;
            }
        }

        private class BigStringInternal : DataType
        {
            public BigStringInternal() : base( "BigString", 16)
            {
            }
            
            public override object DefaultValue => default(string);
            
            public override Type GetClrType()
            {
                return typeof(string);
            }

            public override object Parse(string value)
            {
                return value;
            }

            public override object Parse(string[] value)
            {
                return value;
            }
        }
    }
}