using System.Collections.Specialized;
using System.Windows;
using GClient.Core.Mvvm;
using Prism.Regions;

namespace GClient.Core.Prism.RegionBehaviors
{
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = nameof(RegionManagerAwareBehavior);

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
                        if (element.GetValue(RegionManager.RegionManagerProperty) is IRegionManager scopedRegionManager)
                            regionManager = scopedRegionManager;

                    AssignRegionManagerAware(item, regionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                    AssignRegionManagerAware(item, null);
            }
        }

        private static void AssignRegionManagerAware(object item, IRegionManager regionManager)
        {
            var regionManagerAware = MvvmHelper.GetImplementedType<IRegionManagerAware>(item, avoidInheritedContext:true);
            if (regionManagerAware == null) return;
            regionManagerAware.RegionManager = regionManager;
        }
    }
}
