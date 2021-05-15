using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Services
{
    public static class Mapper
    {
        public static GalaxyObjectData Map(GalaxyObject source)
        {
            return new GalaxyObjectData
            {
                TagName = source.TagName,
                HierarchicalName = source.HierarchicalName
                //todo...
            };
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
    }
}