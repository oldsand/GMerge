using System;
using System.Collections.Generic;
using ArchestrA.GRAccess;
// ReSharper disable SuspiciousTypeConversion.Global because GRAccess IInstance and ITemplate do not implement IgObject (Not sure if this is why)
// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator because GRAccess does not implement IEnumerable (I think)

namespace GServer.Archestra.Extensions
{
    public static class ObjectCollectionExtensions
    {
        public static bool Contains(this IgObjects gObjects, Predicate<IgObject> predicate)
        {
            foreach (IgObject gObject in gObjects)
                if (predicate.Invoke(gObject))
                    return true;

            return false;
        }

        public static IEnumerable<IgObject> Where(this IgObjects gObjects, Predicate<IgObject> predicate)
        {
            foreach (IgObject gObject in gObjects)
                if (predicate(gObject))
                    yield return gObject;
        }

        public static IEnumerable<IgObject> AsEnumerable(this IgObjects gObjects)
        {
            foreach (IgObject gObject in gObjects)
                yield return gObject;
        }
        
        public static IEnumerable<ITemplate> AsTemplates(this IgObjects gObjects)
        {
            foreach (IgObject gObject in gObjects)
                yield return (ITemplate) gObject;
        }
        
        public static IEnumerable<IInstance> AsInstances(this IgObjects gObjects)
        {
            foreach (IgObject gObject in gObjects)
                yield return (IInstance) gObject;
        }

        public static IgObjects AsGObjects(this IEnumerable<ITemplate> templates, IgObjects collection)
        {
            foreach (var template in templates)
                collection.Add((IgObject) template);
            return collection;
        }
        
        public static IgObjects AsGObjects(this IEnumerable<IInstance> instances, IgObjects collection)
        {
            foreach (var instance in instances)
                collection.Add((IgObject) instance);
            return collection;
        }
    }
}