using GalaxyMerge.Client.Dialogs.ViewModels;

namespace GalaxyMerge.Client.Dialogs.Design
{
    public class DesignModels
    {
        public static NewResourceInfoViewModel NewResourceInfoViewModel => new()
        {
            ResourceEntry =
            {
                ResourceName = "Test Resource Name",
                ResourceDescription = "This is the test description for the resource",
                Connection = { NodeName = "TestNode", GalaxyName = "TestGalaxy"}
            }
        };

        public static ResourceSettingsViewModel ResourceSettingsViewModel => new();
        public static ResourceSettingsGeneralViewModel ResourceSettingsGeneralViewModel => new();
    }
}