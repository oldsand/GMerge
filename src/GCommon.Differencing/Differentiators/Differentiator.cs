using GCommon.Differencing.Abstractions;

namespace GCommon.Differencing.Differentiators
{
    public static class Differentiator<T>
    {
        public static IDifferentiator<T> Default => new DefaultDifferentiator<T>();
    }
}