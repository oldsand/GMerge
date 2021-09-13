using System;
using System.Globalization;
using System.Windows.Controls;
using GClient.Core.Converters;
using GClient.Core.Utilities;
using GCommon.Data.Entities;

namespace GClient.Converters
{
    public class ObjectIconConverter : ValueConverter
    {
        public override object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is not GalaxyObject gObject) return null;

            if (gObject.TemplateDefinition == null)
                throw new InvalidOperationException("GObject Template Definition cannot be null");
            
            //var category = ObjectCategory.FromName("One");
            var baseType = gObject.TemplateDefinition.ObjectId;

            /*return category switch
            {
                ObjectCategory.Galaxy => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy"),
                ObjectCategory.Area => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.Area"),
                ObjectCategory.ApplicationEngine => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationEngine"),
                ObjectCategory.ViewEngine => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ViewEngine"),
                ObjectCategory.PlatformEngine => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.PlatformEngine"),
                ObjectCategory.ApplicationObject => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationObject"),
                ObjectCategory.ViewApp => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.InTouchViewApp"),
                // BaseType 9 == DDESuiteLinkClient
                ObjectCategory.IoNetwork when baseType == 9 => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.DdeClient"),
                // BaseType 12 == OPCClient
                ObjectCategory.IoNetwork when baseType == 12 => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.OpcClient"),
                _ => ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationObject")
            };*/

            return ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationObject");
        }
    }
}