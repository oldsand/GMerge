using System.Windows;
using System.Windows.Media;

namespace GClient.Core.Extensions
{
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Attempts to find the most immediate visual ancestor of the specific type.
        /// </summary>
        /// <param name="d">Current dependency object.</param>
        /// <typeparam name="T">The type of the ancestor to find.</typeparam>
        /// <returns></returns>
        public static T FindVisualAncestor<T>(this DependencyObject d) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(d);

            return parent switch
            {
                null => null,
                T found => found,
                _ => FindVisualAncestor<T>(parent)
            };
        }
    }
}