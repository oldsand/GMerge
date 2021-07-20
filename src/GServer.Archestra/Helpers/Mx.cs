using System;
using System.Collections;
using System.Collections.Generic;
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
        public static MxValue Create<T>()
        {
            var type = typeof(T);
            var mxValueClass = new MxValueClass();

            if (type == typeof(bool) || typeof(IEnumerable<bool>).IsAssignableFrom(type))
                mxValueClass.PutBoolean(default);
            else if (type == typeof(int) || typeof(IEnumerable<int>).IsAssignableFrom(type))
                mxValueClass.PutInteger(default);
            else if (type == typeof(double) || typeof(IEnumerable<double>).IsAssignableFrom(type))
                mxValueClass.PutDouble(default);
            else if (type == typeof(float) || typeof(IEnumerable<float>).IsAssignableFrom(type))
                mxValueClass.PutFloat(default);
            else if (type == typeof(string) || typeof(IEnumerable<string>).IsAssignableFrom(type))
                mxValueClass.PutString(default);
            else if (type == typeof(DateTime) || typeof(IEnumerable<DateTime>).IsAssignableFrom(type))
                mxValueClass.PutTime(default);
            else if (type == typeof(TimeSpan) || typeof(IEnumerable<TimeSpan>).IsAssignableFrom(type))
                mxValueClass.PutElapsedTime(default);
            else if (type == typeof(Reference) || typeof(IEnumerable<Reference>).IsAssignableFrom(type))
                mxValueClass.PutMxReference(new MxReference());
            else if (type == typeof(StatusCategory) || typeof(IEnumerable<StatusCategory>).IsAssignableFrom(type))
                mxValueClass.PutMxStatus(new MxStatus());
            else if (type == typeof(DataType) || typeof(IEnumerable<DataType>).IsAssignableFrom(type))
                mxValueClass.PutMxDataType(default);
            else if (type == typeof(SecurityClassification) ||
                     typeof(IEnumerable<SecurityClassification>).IsAssignableFrom(type))
                mxValueClass.PutMxSecurityClassification(default);
            else if (type == typeof(Quality) || typeof(IEnumerable<Quality>).IsAssignableFrom(type))
                mxValueClass.PutMxDataQuality(default);
            else
                throw new NotSupportedException("");

            var mxValue = new MxValueClass();

            if (typeof(IEnumerable).IsAssignableFrom(type))
                mxValue.PutElement(1, mxValueClass);
            else
                mxValue = mxValueClass;

            return mxValue;
        }

        public static MxValue Create<T>(T value)
        {
            var mxValue = Create<T>();
            mxValue.SetValue(value);
            return mxValue;
        }
    }
}