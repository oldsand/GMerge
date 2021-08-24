using System;
using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface IDifferentiable<TSource, TDifference> : IEquatable<TSource>
    {
        IEnumerable<Difference<TDifference>> DiffersFrom(TSource other);
    }
}