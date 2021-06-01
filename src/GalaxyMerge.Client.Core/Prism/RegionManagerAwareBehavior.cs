using System;
using System.Collections.Specialized;
using System.Windows;
using Prism.Regions;

namespace GalaxyMerge.Client.Core.Prism
{
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var regionManager = Region.RegionManager;

                    // If the view was created with a scoped region manager, the behavior uses that region manager instead.
                    if (item is FrameworkElement element)
                    {
                        if (element.GetValue(RegionManager.RegionManagerProperty) is IRegionManager scopedRegionManager)
                        {
                            regionManager = scopedRegionManager;
                        }
                    }

                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        private static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            switch (item)
            {
                case IRegionManagerAware regionManagerAwareItem:
                    invocation(regionManagerAwareItem);
                    break;
                case FrameworkElement frameworkElement:
                {
                    if (frameworkElement.DataContext is not IRegionManagerAware regionManagerAwareDataContext) return;
                
                    // If a view doesn't have a data context (view model) it will inherit the data context from the parent view.
                    // The following check is done to avoid setting the RegionManager property in the view model of the parent view by mistake. 
                    if (frameworkElement.Parent is FrameworkElement frameworkElementParent)
                    {
                        if (frameworkElementParent.DataContext is IRegionManagerAware regionManagerAwareDataContextParent)
                        {
                            if (regionManagerAwareDataContext == regionManagerAwareDataContextParent)
                            {
                                // If all of the previous conditions are true, it means that this view doesn't have a view model
                                // and is using the view model of its visual parent.
                                return;
                            }
                        }
                    }

                    invocation(regionManagerAwareDataContext);
                    break;
                }
            }
        }
    }
}
