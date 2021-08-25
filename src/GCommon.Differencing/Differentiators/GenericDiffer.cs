using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class GenericDiffer<T> : IDifferentiator<T, T>
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

        /// <summary>
        /// Gets the difference in any given type using the default comparer.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public IEnumerable<Difference<T>> DifferenceIn(T first, T second)
        {
            var differences = new List<Difference<T>>();

            if (!Equals(first, second))
                differences.Add(new Difference<T>(first, second));

            return differences;
        }

        public IEnumerable<Difference<T>> DifferenceIn(IEnumerable<T> first, IEnumerable<T> second)
        {
            throw new System.NotImplementedException();
        }
    }
}