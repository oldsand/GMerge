using System.Windows;
using System.Windows.Controls;
using GalaxyMerge.Client.Core.Extensions;
using Microsoft.Xaml.Behaviors;
using Prism.Regions;

namespace GalaxyMerge.Client.Core.Behaviors
{
    public class CloseTabAction : TriggerAction<Button>
    {
        protected override void Invoke(object parameter)
        {
            if (!(parameter is RoutedEventArgs args))
                return;
            
            var tabItem = (args.OriginalSource as DependencyObject).FindVisualAncestor<TabItem>();

            var tabControl = tabItem?.FindVisualAncestor<TabControl>();
            if (tabControl == null)
                return;

            var region = RegionManager.GetObservableRegion(tabControl).Value;
            if (region == null)
                return;

            RemoveItemFromRegion(tabItem.Content, region);
        }

        private static void RemoveItemFromRegion(object item, IRegion region)
        {
            var navigationContext = new NavigationContext(region.NavigationService, null);
            if (!CanRemove(item, navigationContext)) return;
            InvokeOnNavigatedFrom(item, navigationContext);
            region.Remove(item);
        }

        private static void InvokeOnNavigatedFrom(object item, NavigationContext navigationContext)
        {
            switch (item)
            {
                case INavigationAware navigationAwareItem:
                    navigationAwareItem.OnNavigatedFrom(navigationContext);
                    break;
                case FrameworkElement frameworkElement:
                {
                    if (frameworkElement.DataContext is INavigationAware navigationAwareDataContext)
                        navigationAwareDataContext.OnNavigatedFrom(navigationContext);
                    break;
                }
            }
        }

        private static bool CanRemove(object item, NavigationContext navigationContext)
        {
            var canRemove = true;

            if (item is IConfirmNavigationRequest confirmRequestItem)
            {
                confirmRequestItem.ConfirmNavigationRequest(navigationContext, result =>
                {
                    canRemove = result;
                });
            }

            if (item is FrameworkElement frameworkElement && canRemove)
            {
                if (frameworkElement.DataContext is IConfirmNavigationRequest confirmRequestDataContext)
                {
                    confirmRequestDataContext.ConfirmNavigationRequest(navigationContext, result =>
                    {
                        canRemove = result;
                    });
                }
            }

            return canRemove;
        }
    }
}