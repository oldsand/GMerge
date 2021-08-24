using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface IDifferentiator<TSource, TDifference>
    {
        IEnumerable<Difference<TDifference>> DifferenceIn(TSource first, TSource second,
            IEqualityComparer<TSource> comparer = null); 
    }
}