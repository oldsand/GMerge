using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.SmartEnum;
using GCommon.Primitives.Helpers;
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
        
        public virtual object Parse(string value)
        {
            return null;
        }

        public virtual object Parse(string[] value)
        {
            return default;
        }

        public object Parse(Hex hex)
        {
            var parser = new HexParser(hex);

            return parser.IsArray ? ParseArray(parser.ParseDataArray()) : ParseSingle(parser.Data);
        }

        protected virtual object ParseSingle(Hex hex)
        {
            return hex.Reverse().Value;
        }

        protected virtual object ParseArray(IEnumerable<Hex> hexes)
        {
            return hexes.Select(Parse);
        }

        #region InternalClasses

        private class UnknownInternal : DataType
        {
            public UnknownInternal() : base( "Unknown", -1)
            {
            }

            protected override object ParseSingle(Hex hex)
            {
                return null;
            }
        }

        private class NoDataInternal : DataType
        {
            public NoDataInternal() : base("No Data", 0)
            {
            }
            
            protected override object ParseSingle(Hex hex)
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

            protected override object ParseSingle(Hex hex)
            {
                return hex.Reverse().ToBool();
            }
        }

        private class IntegerInternal : DataType
        {
            public IntegerInternal() : base("Integer", 2)
            {
            }
            
            public override object DefaultValue => default(int);

            protected override object ParseSingle(Hex hex)
            {
                return hex.Reverse().ToInt();
            }
        }

        private class FloatInternal : DataType
        {
            public FloatInternal() : base("Float", 3)
            {
            }

            public override object DefaultValue => default(float);
            
            protected override object ParseSingle(Hex hex)
            {
                return hex.Reverse().ToFloat();
            }
        }

        private class DoubleInternal : DataType
        {
            public DoubleInternal() : base("Double", 4)
            {
            }

            public override object DefaultValue => default(double);
            
            protected override object ParseSingle(Hex hex)
            {
                return hex.Reverse().ToDouble();
            }
        }

        private class StringInternal : DataType
        {
            public StringInternal() : base("String", 5)
            {
            }
            
            public override object DefaultValue => string.Empty;
            
            protected override object ParseSingle(Hex hex)
            {
                var builder = new StringBuilder();
                
                //the data we will get is expected to have a 16 bit header so we need to strip it
                var chars = hex.DropHead(16).ToEnumerable(4);
                
                foreach (var c in chars)
                    builder.Append(c.Reverse().ToChar());

                return builder.ToString();
            }
        }

        private class TimeInternal : DataType
        {
            public TimeInternal() : base("Time", 6)
            {
            }
            
            public override object DefaultValue => DateTime.MinValue;
            
            protected override object ParseSingle(Hex hex)
            {
                return hex.Reverse().ToDateTime();
            }

            protected override object ParseArray(IEnumerable<Hex> hexes)
            {
                //For some reason date time arrays have extra 4 bits unknown data, so we need to trim it.
                return hexes.Select(h => Parse(h.DropTail(4)));
            }
        }

        private class ElapsedTimeInternal : DataType
        {
            public ElapsedTimeInternal() : base("ElapsedTime", 7)
            {
            }
            
            public override object DefaultValue => TimeSpan.MinValue;
            
            protected override object ParseSingle(Hex hex)
            {
                return hex.Reverse().ToTimeSpan();
            }
        }

        private class ReferenceTypeInternal : DataType
        {
            public ReferenceTypeInternal() : base("ReferenceType", 8)
            {
            }
            
            public override object DefaultValue => Reference.Empty();

            protected override object ParseSingle(Hex hex)
            {
                var builder = new StringBuilder();
                
                //The first 8 are the entire length which we don't need. The last 40 are always nothing.
                hex = hex.DropHead(8).DropTail(40);

                var refLength = hex.Consume(8).DropTail(4).Reverse().ToInt();
                var refChars = hex.Consume(refLength).ToEnumerable(4);

                foreach (var c in refChars)
                    builder.Append(c.Reverse().ToChar());

                var fullReference = builder.ToString();

                builder.Clear();
                
                var objLength = hex.Consume(8).DropTail(4).Reverse().ToInt();
                var objChars = hex.Consume(objLength).ToEnumerable(4);
                
                foreach (var c in objChars)
                    builder.Append(c.Reverse().ToChar());

                var objReference = builder.ToString();

                return Reference.FromName(fullReference);
            }
        }
        
        private class StatusTypeInternal : DataType
        {
            public StatusTypeInternal() : base("Status", 9)
            {
            }
            
            public override object DefaultValue => StatusCategory.Unknown;
        }

        private class DataTypeEnumInternal : DataType
        {
            public DataTypeEnumInternal() : base( "DataTypeEnum", 10)
            {
            }
            
            public override object DefaultValue => NoData;

            protected override object ParseSingle(Hex hex)
            {
                return FromValue(hex.Reverse().ToInt());
            }
        }
        
        private class SecurityClassificationEnumInternal : DataType
        {
            public SecurityClassificationEnumInternal() : base( "SecurityClassificationEnum", 11)
            {
            }
            
            public override object DefaultValue => SecurityClassification.Undefined;
        }
        
        private class DataQualityInternal : DataType
        {
            public DataQualityInternal() : base( "DataQuality", 12)
            {
            }
            
            public override object DefaultValue => Quality.Unknown;
        }

        private class QualifiedEnumInternal : DataType
        {
            public QualifiedEnumInternal() : base( "QualifiedEnum", 13)
            {
            }
            
            public override object DefaultValue => default(Enumeration);
        }
        
        private class QualifiedStructInternal : DataType
        {
            public QualifiedStructInternal() : base( "QualifiedStruct", 14)
            {
            }
            
            public override object DefaultValue => Blob.Empty();
        }

        private class InternationalizedStringInternal : DataType
        {
            public InternationalizedStringInternal() : base( "InternationalizedStruct", 15)
            {
            }
            
            public override object DefaultValue => default(string);
        }

        private class BigStringInternal : DataType
        {
            public BigStringInternal() : base( "BigString", 16)
            {
            }
            
            public override object DefaultValue => default(string);
        }

        #endregion
        
    }
}