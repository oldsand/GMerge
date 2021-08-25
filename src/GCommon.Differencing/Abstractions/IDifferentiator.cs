using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface IDifferentiator<in TSource, TDifference> : IEqualityComparer<TSource>
    {
        IEnumerable<Difference<TDifference>> DifferenceIn(TSource first, TSource second);
    }
}