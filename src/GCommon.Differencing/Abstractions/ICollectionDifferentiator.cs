using System;
using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface ICollectionDifferentiator<TSource, TDifference> : IEqualityComparer<TSource>
    {
        IEnumerable<Difference<TDifference>> DifferenceIn<TValue>(IEnumerable<TSource> first,
            IEnumerable<TSource> second, Func<TSource, TValue> key, CollectionMatchMode mode);
    }
}