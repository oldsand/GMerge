using System;
using System.Globalization;
using System.Windows.Controls;
using GalaxyMerge.Client.Core.Converters;
using GalaxyMerge.Client.Core.Utilities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Client.Converters
{
    public class ObjectCategoryToIconConverter : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not ObjectCategory) return null;
            
            if (values[1] is not int) return null;

            var category = (ObjectCategory) values[0];
            var baseType = (int) values[1];

            if (category == ObjectCategory.Galaxy)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy");
            
            if (category == ObjectCategory.Area)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.Area");
            
            if (category == ObjectCategory.ApplicationEngine)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationEngine");
            
            if (category == ObjectCategory.ViewEngine)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ViewEngine");
            
            if (category == ObjectCategory.PlatformEngine)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.PlatformEngine");
            
            if (category == ObjectCategory.ApplicationObject)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationObject");
            
            if (category == ObjectCategory.ViewApp)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.InTouchViewApp");
            
            if (category == ObjectCategory.IoNetwork && baseType == 9) // BaseType 9 == DDESuiteLinkClient
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.DdeClient");
            
            if (category == ObjectCategory.IoNetwork && baseType == 12) // BaseType 12 == OPCClient
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.OpcClient");
            
            /*if (category == ObjectCategory.PostIo)
                return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.RdiObject");*/

            //Default to application object icon for unknown types
            return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationObject");
        }
    }
}