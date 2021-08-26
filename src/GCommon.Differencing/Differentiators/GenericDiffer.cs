using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class GenericDiffer<T> : IDifferentiator<T>
    {
        private readonly IEqualityComparer<T> _customComparer;

        public GenericDiffer(IEqualityComparer<T> customComparer = null)
        {
            _customComparer = customComparer;
        }

        public bool Equals(T x, T y)
        {
            var comparer = _customComparer ?? EqualityComparer<T>.Default;
            return comparer.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public IEnumerable<Difference> DifferenceIn(T first, T second)
        {
            /*if (first is IDifferentiable<T> differentiable)
            {
                return differentiable.DiffersFrom(second);
            }*/
            
            var differences = new List<Difference>();

            if (!Equals(first, second))
                differences.Add(Difference.Create(first, second));

            return differences;
        }
    }
}