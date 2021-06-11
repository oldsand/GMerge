using System;
using System.Globalization;
using System.Windows.Controls;
using GalaxyMerge.Client.Core.Converters;
using GalaxyMerge.Client.Core.Utilities;
using GalaxyMerge.Client.Data.Entities;

namespace GalaxyMerge.Client.Converters
{
    public class ResourceTypeIconConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ResourceType resourceType)
                throw new InvalidOperationException("Value must be of type ResourceType");

            return resourceType switch
            {
                ResourceType.Connection => ResourceFinder.Find<ControlTemplate>("Icon.Connection"),
                ResourceType.Archive => ResourceFinder.Find<ControlTemplate>("Icon.Archive"),
                ResourceType.Directory => ResourceFinder.Find<ControlTemplate>("Icon.File"),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}