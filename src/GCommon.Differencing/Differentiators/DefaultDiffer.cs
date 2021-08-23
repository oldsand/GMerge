using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class DefaultDiffer<T> : IDifferentiator<T>
    {
        public bool Equals(T x, T y)
        {
            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public IEnumerable<Difference> DifferenceIn(T first, T second)
        {
            var differences = new List<Difference>();
            
            if (!Equals(first, second))
            {
                differences.Add(Difference.Create(first, second, x => x));
            }

            return differences;
        }
    }
}