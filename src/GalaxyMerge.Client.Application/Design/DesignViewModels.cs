using GalaxyMerge.Client.Application.ViewModels;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;

namespace GalaxyMerge.Client.Application.Design
{
    public class DesignViewModels
    {
        public static ShellHeaderViewModel ShellHeaderViewModel=> new ShellHeaderViewModel();
        public static ResourceEntryWrapper ResourceEntry => new(new ResourceEntry("ResourceName", ResourceType.Connection));
    }
}