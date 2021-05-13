// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace GalaxyMerge.Archive.Entities
{
    public class GalaxyInfo
    {
        private GalaxyInfo()
        {
        }
        
        public GalaxyInfo(string galaxyName)
        {
            GalaxyName = galaxyName;
            
            //Defaulting to SP2014R2SP1 for now
            VersionNumber = 37;
            CdiVersion = "3735.0233.0399.0061";
            IsaVersion = "4.1.12100";
        }
        
        public GalaxyInfo(string galaxyName, int? versionNumber, string cdiVersion, string isaVersion)
        {
            GalaxyName = galaxyName;
            VersionNumber = versionNumber;
            CdiVersion = cdiVersion;
            IsaVersion = isaVersion;
        }
        
        public string GalaxyName { get; private set; }
        public int? VersionNumber { get; private set; }
        public string CdiVersion { get; private set; }
        public string IsaVersion { get; private set; }
    }
}