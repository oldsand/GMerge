using System;

namespace GClient.Core.Prism
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependentViewAttribute : Attribute
    {
        public Type Type { get; }
        public string TargetRegionName { get; }

        public DependentViewAttribute(Type viewType, string targetRegionName)
        {
            Type = viewType;
            TargetRegionName = targetRegionName;
        }
    }
}