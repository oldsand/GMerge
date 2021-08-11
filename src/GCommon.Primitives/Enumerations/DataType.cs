using System;
using Ardalis.SmartEnum;

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
        public static readonly DataType StatusType = new StatusTypeInternal();
        public static readonly DataType DataTypeEnum = new DataTypeEnumInternal();
        public static readonly DataType SecurityClassificationEnum = new SecurityClassificationEnumInternal();
        public static readonly DataType DataQuality = new DataQualityInternal();
        public static readonly DataType QualifiedEnum = new QualifiedEnumInternal();
        public static readonly DataType QualifiedStruct = new QualifiedStructInternal();
        public static readonly DataType InternationalizedString = new InternationalizedStringInternal();
        public static readonly DataType BigString = new BigStringInternal();

        public virtual object DefaultValue => default;
        public abstract Type GetClrType();

        private class UnknownInternal : DataType
        {
            public UnknownInternal() : base( "Unknown", -1)
            {
            }

            public override Type GetClrType()
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
        }
        
        private class StatusTypeInternal : DataType
        {
            public StatusTypeInternal() : base("StatusType", 9)
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(StatusCategory);
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
        }
    }
}