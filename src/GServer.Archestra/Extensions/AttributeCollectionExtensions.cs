using System;
using System.Collections.Generic;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

namespace GServer.Archestra.Extensions
{
    public static class AttributeCollectionExtensions
    {
        public static IEnumerable<ArchestraAttribute> Map(this IAttributes attributes)
        {
            foreach (IAttribute attribute in attributes)
                yield return attribute.Map();
        }
        
        public static IEnumerable<IAttribute> Where(this IAttributes attributes, Predicate<IAttribute> predicate)
        {
            foreach (IAttribute attribute in attributes)
                if (predicate(attribute))
                    yield return attribute;
        }
        
        public static IEnumerable<ArchestraAttribute> ByDataType(this IAttributes attributes, DataType dataType)
        {
            foreach (IAttribute attribute in attributes)
                if (attribute.DataType == dataType.ToMx())
                    yield return attribute.Map();
        }
        
        public static IEnumerable<ArchestraAttribute> ByNameContains(this IAttributes attributes, string name)
        {
            foreach (IAttribute attribute in attributes)
                if (attribute.Name.Contains(name))
                    yield return attribute.Map();
        }
    }
}