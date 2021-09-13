namespace GCommon.Core.Abstractions
{
    public interface IDuplicable<out T>
    {
        T Duplicate();
    }
}