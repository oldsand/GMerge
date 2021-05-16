using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Services
{
    public static class Mapper
    {
        public static GalaxyObjectData Map(GalaxyObject source)
        {
            return MapGalaxyObject(source);
        }

        public static GalaxySymbolData Map(GalaxySymbol source)
        {
            return new GalaxySymbolData
            {
                //todo...
            };
        }
        
        public static CustomPropertyData Map(CustomProperty source)
        {
            return new CustomPropertyData   
            {
                //todo...
            };
        }
        
        private static GalaxyObjectData MapGalaxyObject(GalaxyObject source)
        {
            return new GalaxyObjectData
            {
                TagName = source.TagName,
                ContainedName = source.ContainedName,
                HierarchicalName = source.HierarchicalName,
                ConfigVersion = source.ConfigVersion,
                DerivedFromName = source.DerivedFromName,
                BasedOnName = source.BasedOnName,
                Category = source.Category,
                HostName = source.HostName,
                AreaName = source.AreaName,
                ContainerName = source.ContainerName,
                Attributes = MapAttributes(source.Attributes)
            };
        }

        private static GalaxyAttributeData MapAttribute(GalaxyAttribute source)
        {
            return new GalaxyAttributeData
            {
                Name = source.Name,
                DataType = source.DataType,
                Category = source.Category,
                Security = source.Security,
                Locked = source.Locked,
                Value = source.Value,
                ArrayCount = source.ArrayCount
            };
        }

        private static IEnumerable<GalaxyAttributeData> MapAttributes(IEnumerable<GalaxyAttribute> attributes)
        {
            return attributes.Select(MapAttribute);
        }
    }
}