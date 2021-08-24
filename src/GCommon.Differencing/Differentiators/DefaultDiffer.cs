using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class DefaultDiffer<T> : IDifferentiator<T, T>
    {
        public IEnumerable<Difference<T>> DifferenceIn(T first, T second, IEqualityComparer<T> comparer = null)
        {
            comparer ??= EqualityComparer<T>.Default;
            var differences = new List<Difference<T>>();
            
            if (!comparer.Equals(first, second))
                differences.Add(Difference<T>.Create(first, second, x => x));

            return differences;
        }
    }
}