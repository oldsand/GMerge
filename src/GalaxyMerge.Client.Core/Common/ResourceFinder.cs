using System.Windows;

namespace GalaxyMerge.Client.Core.Common
{
    public static class ResourceFinder
    {
        public static T Find<T>(string resourceName) where T : class
        {
            return Application.Current.TryFindResource(resourceName) as T;
        }
    }
}