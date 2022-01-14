namespace GServer.Archestra.Exceptions
{
    public class ObjectNotCheckedOutToMeException : GalaxyException
    {
        public ObjectNotCheckedOutToMeException(string message) : base(message)
        {
        }
    }
}