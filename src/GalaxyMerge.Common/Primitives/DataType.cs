using System;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Primitives
{
    public abstract class DataType : Enumeration
    {
        // ReSharper disable once UnusedMember.Local
        // Used for object serialization
        private DataType()
        {
        }

        private DataType(int id, string name) : base(id, name)
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

        public abstract Type GetClrType();

        private class UnknownInternal : DataType
        {
            public UnknownInternal() : base(-1, "Unknown")
            {
            }

            public override Type GetClrType()
            {
                return null;
            }
        }

        private class NoDataInternal : DataType
        {
            public NoDataInternal() : base(0, "No Data")
            {
            }

            public override Type GetClrType()
            {
                return null;
            }
        }

        private class BooleanInternal : DataType
        {
            public BooleanInternal() : base(1, "Boolean")
            {
            }

            public override Type GetClrType()
            {
                return typeof(bool);
            }
        }

        private class IntegerInternal : DataType
        {
            public IntegerInternal() : base(2, "Integer")
            {
            }

            public override Type GetClrType()
            {
                return typeof(int);
            }
        }

        private class FloatInternal : DataType
        {
            public FloatInternal() : base(3, "Float")
            {
            }

            public override Type GetClrType()
            {
                return typeof(float);
            }
        }

        private class DoubleInternal : DataType
        {
            public DoubleInternal() : base(4, "Double")
            {
            }

            public override Type GetClrType()
            {
                return typeof(double);
            }
        }

        private class StringInternal : DataType
        {
            public StringInternal() : base(5, "String")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
        }

        private class TimeInternal : DataType
        {
            public TimeInternal() : base(6, "Time")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(DateTime);
            }
        }

        private class ElapsedTimeInternal : DataType
        {
            public ElapsedTimeInternal() : base(7, "ElapsedTime")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(TimeSpan);
            }
        }

        private class ReferenceTypeInternal : DataType
        {
            public ReferenceTypeInternal() : base(8, "ReferenceType")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
        }
        
        private class StatusTypeInternal : DataType
        {
            public StatusTypeInternal() : base(9, "StatusType")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(double);
            }
        }

        private class DataTypeEnumInternal : DataType
        {
            public DataTypeEnumInternal() : base(10, "DataTypeEnum")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(DataType);
            }
        }
        
        private class SecurityClassificationEnumInternal : DataType
        {
            public SecurityClassificationEnumInternal() : base(11, "SecurityClassificationEnum")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(SecurityClassification);
            }
        }
        
        private class DataQualityInternal : DataType
        {
            public DataQualityInternal() : base(12, "DataQuality")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(Quality);
            }
        }

        private class QualifiedEnumInternal : DataType
        {
            public QualifiedEnumInternal() : base(13, "QualifiedEnum")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
        }
        
        private class QualifiedStructInternal : DataType
        {
            public QualifiedStructInternal() : base(14, "QualifiedStruct")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(byte[]);
            }
        }

        private class InternationalizedStringInternal : DataType
        {
            public InternationalizedStringInternal() : base(15, "InternationalizedStruct")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
        }

        private class BigStringInternal : DataType
        {
            public BigStringInternal() : base(16, "BigString")
            {
            }
            
            public override Type GetClrType()
            {
                return typeof(string);
            }
        }
    }
}