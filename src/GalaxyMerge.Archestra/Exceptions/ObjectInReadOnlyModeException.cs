namespace GalaxyMerge.Archestra.Exceptions
{
    public class ObjectInReadOnlyModeException : GalaxyException
    {
        public ObjectInReadOnlyModeException(string message) : base(message)
        {
        }
    }
}