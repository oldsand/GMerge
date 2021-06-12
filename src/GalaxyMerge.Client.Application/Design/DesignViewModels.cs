using GalaxyMerge.Client.Application.ViewModels;
using GalaxyMerge.Client.Data.Entities;

namespace GalaxyMerge.Client.Application.Design
{
    public class DesignViewModels
    {
        public static ShellHeaderViewModel ShellHeaderViewModel=> new ShellHeaderViewModel();
        public static ResourceEntry ResourceEntry => new ResourceEntry("ResourceName", ResourceType.Connection);
    }
}