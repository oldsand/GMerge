// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveInfo
    {
        public ArchiveInfo(string galaxyName, int versionNumber, string cdiVersion, string isaVersion)
        {
            GalaxyName = galaxyName;
            VersionNumber = versionNumber;
            CdiVersion = cdiVersion;
            IsaVersion = isaVersion;
        }
        
        public string GalaxyName { get; private set; }
        public int VersionNumber { get; private set; }
        public string CdiVersion { get; private set; }
        public string IsaVersion { get; private set; }
    }
}