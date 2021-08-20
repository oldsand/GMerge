using System;
using System.Collections.Generic;

namespace GCommon.Differencing.Abstractions
{
    public interface IDifferentiable<T> : IEquatable<T>
    {
        IEnumerable<Difference> DiffersFrom(T other);
    }
}