using System;

namespace GalaxyMerge.Data.Entities
{
    public class Galaxy
    {
        public string Name { get; set; }
        public string IsaVersion { get; set; }
        public string CdiVersion { get; set; }
        public string VersionNumber { get; set; }
        public DateTime LastDeployment { get; set; }
        public DateTime LastConfigurationChange { get; set; }
        public bool IsGalaxyInstalled { get; set; }
    }
}