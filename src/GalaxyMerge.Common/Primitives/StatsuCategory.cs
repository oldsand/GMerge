namespace GalaxyMerge.Common.Primitives
{
    public enum StatusCategory
    {
        Unknown = -1,
        Ok = 0,
        Pending = 1,
        Warning = 2,
        CommunicationError = 3,
        ConfigurationError = 4,
        OperationalError = 5,
        SecurityError = 6,
        SoftwareError = 7,
        OtherError = 8,
    }
}