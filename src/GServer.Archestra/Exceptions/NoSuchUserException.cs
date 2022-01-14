namespace GServer.Archestra.Exceptions
{
    public class NoSuchUserException : GalaxyException
    {
        public NoSuchUserException(string message) : base(message)
        {
        }
    }
}