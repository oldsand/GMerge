using GCommon.Differencing.Abstractions;
using GCommon.Differencing.Differentiators;

namespace GCommon.Differencing
{
    public static class Differentiator<T>
    {
        public static IDifferentiator<T, T> Default => new DefaultDiffer<T>();
    }
}