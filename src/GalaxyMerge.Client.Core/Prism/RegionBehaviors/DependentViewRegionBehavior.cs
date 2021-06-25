using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using GalaxyMerge.Client.Core.Mvvm;
using Prism.Regions;

namespace GalaxyMerge.Client.Core.Prism.RegionBehaviors
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        private readonly Dictionary<object, List<DependentViewInfo>> _dependentViewCache = new();

        public const string BehaviorKey = nameof(DependentViewRegionBehavior);

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newView in e.NewItems)
                {
                    var dependentViews = FindDependentViews(newView);
                    dependentViews.ForEach(item => Region.RegionManager.Regions[item.TargetRegionName].Add(item.View));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldView in e.OldItems)
                {
                    if (!_dependentViewCache.ContainsKey(oldView)) continue;
                    
                    _dependentViewCache[oldView].ForEach(x => Region.RegionManager.Regions[x.TargetRegionName].Remove(x.View));

                    if (!ShouldKeepAlive(oldView))
                        _dependentViewCache.Remove(oldView);
                }
            }
        }

        private List<DependentViewInfo> FindDependentViews(object newView)
        {
            if (_dependentViewCache.ContainsKey(newView))
                return _dependentViewCache[newView];

            var dependentViews = new List<DependentViewInfo>();
            var dependentViewAttributes = MvvmHelper.GetCustomAttributes<DependentViewAttribute>(newView.GetType());
            
            foreach (var dependentViewAttribute in dependentViewAttributes)
            {
                var dependentViewInfo = CreateDependentViewInfo(dependentViewAttribute);
                AssignDataContext(dependentViewInfo.View, newView);
                dependentViews.Add(dependentViewInfo);
            }

            if (!_dependentViewCache.ContainsKey(newView))
                _dependentViewCache.Add(newView, dependentViews);

            return dependentViews;
        }

        private static void AssignDataContext(object dependentView, object newView)
        {
            var newViewDataContext = MvvmHelper.GetImplementedType<ISupportDataContext>(newView);
            var dependentDataContext = MvvmHelper.GetImplementedType<ISupportDataContext>(dependentView);
            if (dependentDataContext != null && newViewDataContext != null)
                dependentDataContext.DataContext = newViewDataContext.DataContext;
        }

        private static DependentViewInfo CreateDependentViewInfo(DependentViewAttribute attribute)
        {
            var dependentViewInfo = new DependentViewInfo {TargetRegionName = attribute.TargetRegionName};
            
            if (attribute.Type != null)
                dependentViewInfo.View = Activator.CreateInstance(attribute.Type);

            return dependentViewInfo;
        }

        private static bool ShouldKeepAlive(object view)
        {
            var lifetime = MvvmHelper.GetImplementedType<IRegionMemberLifetime>(view);
            if (lifetime != null)
                return lifetime.KeepAlive;

            var lifetimeAttribute = MvvmHelper.GetImplementedAttribute<RegionMemberLifetimeAttribute>(view);
            return lifetimeAttribute == null || lifetimeAttribute.KeepAlive;
        }
    }

    internal class DependentViewInfo
    {
        public object View { get; set; }
        public string TargetRegionName { get; set; }
    }
}
