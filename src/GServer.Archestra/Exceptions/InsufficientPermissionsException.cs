namespace GServer.Archestra.Exceptions
{
    public class InsufficientPermissionsException : GalaxyException
    {
        public InsufficientPermissionsException(string message) : base(message)
        {
        }
    }
}