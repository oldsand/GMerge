using GClient.Data.Entities;
using GClient.Module.Dialogs.ViewModels;
using GClient.Wrappers;

namespace GClient.Module.Dialogs.Design
{
    public static class DesignModels
    {
        public static readonly ResourceEntryWrapper ResourceEntry = new(
            new ResourceEntry("Test Resource Name", ResourceType.Connection,
                "This is the test description for the resource")); 
        public static NewResourceInfoViewModel NewResourceInfoViewModel => new()
        {
            ResourceEntry =
            {
                ResourceName = "Test Resource Name",
                ResourceDescription = "This is the test description for the resource",
                Connection = { NodeName = "TestNode", GalaxyName = "TestGalaxy"}
            }
        };
        public static ResourceSettingsDialogModel ResourceSettingsDialogModel => new()
        {
            ResourceEntry = ResourceEntry
        };
        public static ResourceSettingsGeneralViewModel ResourceSettingsGeneralViewModel => new()
        {
            ResourceEntry = ResourceEntry
        };
    }
}