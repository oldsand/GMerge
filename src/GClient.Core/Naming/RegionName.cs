
namespace GClient.Core.Naming
{
    public static class RegionName
    {
        //Root Shell Regions
        public const string ShellHeaderRegion = nameof(ShellHeaderRegion);
        public const string ShellContentRegion = nameof(ShellContentRegion);
        public const string ShellFooterRegion = nameof(ShellFooterRegion);

        //Common regions names for Scoped Regions Managers
        public const string ContentRegion = nameof(ContentRegion);
        public const string TabRegion = nameof(TabRegion);
        public const string NavigationRegion = nameof(NavigationRegion);
        public const string DetailsRegion = nameof(DetailsRegion);
        public const string ButtonRegion = nameof(ButtonRegion);

        
        
        //Application Specific
        public const string EventLogRegion = nameof(EventLogRegion);
    }
}
