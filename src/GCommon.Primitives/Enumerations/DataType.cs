using System;
using System.Collections.Generic;
using System.Linq;
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
            return DefaultValue;
        }

        public virtual object Parse(IEnumerable<string> values)
        {
            return values.Select(Parse);
        }

        public object Parse(Hex hex)
        {
            var parser = new HexParser(hex);

            return parser.IsArray ? ParseArray(parser.ParseDataArray()) : ParseSingle(parser.Data);
        }

        protected virtual object ParseSingle(Hex hex)
        {
            return hex.Head(4) == "0x00" ? Hex.Empty : DefaultValue;
        }

        protected virtual object ParseArray(IEnumerable<Hex> hexes)
        {
            return hexes.Select(ParseSingle).ToArray();
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

            public override object Parse(string value)
            {
                bool.TryParse(value, out var result);
                return result;
            }

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
            
            public override object Parse(string value)
            {
                int.TryParse(value, out var result);
                return result;
            }

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
            
            public override object Parse(string value)
            {
                float.TryParse(value, out var result);
                return result;
            }
            
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
            
            public override object Parse(string value)
            {
                double.TryParse(value, out var result);
                return result;
            }
            
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
            
            public override object Parse(string value)
            {
                return value;
            }
            
            protected override object ParseSingle(Hex hex)
            {
                if (hex.Length == 0)
                    return string.Empty;
                
                //the data we will get is expected to have a 16 bit header so we need to strip it
                var chars = hex.DropHead(16).DropTail(4).ToEnumerable(4);

                return chars.FormString();
            }
        }

        private class TimeInternal : DataType
        {
            public TimeInternal() : base("Time", 6)
            {
            }
            
            public override object DefaultValue => DateTime.Now;
            
            public override object Parse(string value)
            {
                DateTime.TryParse(value, out var result);
                return result;
            }
            
            protected override object ParseSingle(Hex hex)
            {
                //for single values there are 8 bits of unknown data at the beginning,
                //but not when it is an element of an array. So taking the last 20 should work
                //since we know that the element size is 20
                return hex.Tail(20).Reverse().ToDateTime();
            }

            protected override object ParseArray(IEnumerable<Hex> hexes)
            {
                //For some reason date time arrays have extra 4 bits unknown data, so we need to trim it.
                return hexes.Select(h => ParseSingle(h.DropTail(4)));
            }
        }

        private class ElapsedTimeInternal : DataType
        {
            public ElapsedTimeInternal() : base("ElapsedTime", 7)
            {
            }
            
            public override object DefaultValue => TimeSpan.Zero;
            
            public override object Parse(string value)
            {
                TimeSpan.TryParse(value, out var result);
                return result;
            }
            
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
            
            public override object DefaultValue => Reference.Empty;
            
            public override object Parse(string value)
            {
                return new Reference(value);
            }

            protected override object ParseSingle(Hex hex)
            {
                if (hex.Length == 0)
                    return string.Empty;

                //The first 8 are the entire length which we don't need. The last 40 are always nothing.
                hex = hex.DropHead(8).DropTail(40);

                var refLength = hex.Consume(8).DropTail(4).Reverse().ToInt() * 2;
                if (refLength == 0)
                    return Reference.Empty;
                
                var refChars = hex.Consume(refLength).DropTail(4).ToEnumerable(4);
                var fullReference = refChars.FormString();

                if (hex.Length == 0)
                    return new Reference(fullReference);
                
                //Then there should be 16 bits of padding before the object name.
                hex = hex.DropHead(16);
                
                var objLength = hex.Consume(8).DropTail(4).Reverse().ToInt() * 2;
                if (objLength == 0)
                    return new Reference(fullReference);
                
                var objChars = hex.Consume(objLength).DropTail(4).ToEnumerable(4);
                var objReference = objChars.FormString();
                    
                return new Reference(fullReference, objReference);
            }
        }
        
        private class StatusTypeInternal : DataType
        {
            public StatusTypeInternal() : base("Status", 9)
            {
            }
            
            public override object Parse(string value)
            {
                return StatusCategory.FromName(value);
            }
            
            public override object DefaultValue => StatusCategory.Unknown;
        }

        private class DataTypeEnumInternal : DataType
        {
            public DataTypeEnumInternal() : base( "DataTypeEnum", 10)
            {
            }
            
            public override object DefaultValue => NoData;

            public override object Parse(string value)
            {
                return FromName(value);
            }

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
            
            public override object Parse(string value)
            {
                return SecurityClassification.FromName(value);
            }
        }
        
        private class DataQualityInternal : DataType
        {
            public DataQualityInternal() : base( "DataQuality", 12)
            {
            }
            
            public override object DefaultValue => Quality.Unknown;
            
            public override object Parse(string value)
            {
                return Quality.FromName(value);
            }
        }

        private class QualifiedEnumInternal : DataType
        {
            public QualifiedEnumInternal() : base( "QualifiedEnum", 13)
            {
            }
            
            public override object DefaultValue => default(Enumeration);
            
            public override object Parse(string value)
            {
                return new Enumeration(value);
            }
        }
        
        private class QualifiedStructInternal : DataType
        {
            public QualifiedStructInternal() : base( "QualifiedStruct", 14)
            {
            }
            
            public override object DefaultValue => Blob.Empty();
            
            public override object Parse(string value)
            {
                return new Blob();
            }
        }

        private class InternationalizedStringInternal : DataType
        {
            public InternationalizedStringInternal() : base( "InternationalizedStruct", 15)
            {
            }
            
            public override object DefaultValue => default(string);
            
            public override object Parse(string value)
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
            
            public override object Parse(string value)
            {
                return value;
            }
        }

        #endregion
        
    }
}