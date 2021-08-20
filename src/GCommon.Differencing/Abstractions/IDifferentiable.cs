using System;
using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface IDifferentiable<T> : IEquatable<T>, IComparable<T>, IComparable
    {
        IEnumerable<Difference> DiffersFrom(T other);
    }
}