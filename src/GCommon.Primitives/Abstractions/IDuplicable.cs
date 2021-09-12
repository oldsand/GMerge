namespace GCommon.Primitives.Abstractions
{
    public interface IDuplicable<out T>
    {
        T Duplicate();
    }
}