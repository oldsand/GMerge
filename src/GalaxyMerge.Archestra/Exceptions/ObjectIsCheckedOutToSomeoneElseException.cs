namespace GalaxyMerge.Archestra.Exceptions
{
    public class ObjectIsCheckedOutToSomeoneElseException : GalaxyException
    {
        public ObjectIsCheckedOutToSomeoneElseException(string message) : base(message)
        {
        }
    }
}