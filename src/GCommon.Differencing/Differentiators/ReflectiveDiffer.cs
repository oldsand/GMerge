using System.Collections.Generic;
using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public class ReflectiveDiffer<T> : IDifferentiator<T>
    {
        public bool Equals(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Difference> DifferenceIn(T first, T second)
        {
            throw new System.NotImplementedException();
        }
    }
}