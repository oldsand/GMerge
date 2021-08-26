using System;
using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class GenericDiffer<T> : IDifferentiator<T>
    {
        private readonly string _propertyName;
        private readonly Type _parentObject;
        private readonly IEqualityComparer<T> _customComparer;
        
        public GenericDiffer(string propertyName, Type parentObject, IEqualityComparer<T> customComparer = null)
        {
            _propertyName = propertyName;
            _parentObject = parentObject;
            _customComparer = customComparer;
        }

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
            var differences = new List<Difference>();

            if (!Equals(first, second))
                differences.Add(Difference.Create(first, second, _propertyName, _parentObject));

            return differences;
        }
    }
}