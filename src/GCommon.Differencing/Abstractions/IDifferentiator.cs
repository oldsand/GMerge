using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface IDifferentiator<in T> : IEqualityComparer<T>
    {
        IEnumerable<Difference> DifferenceIn(T first, T second);    
    }
}