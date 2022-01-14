using System.Reflection;

namespace GCommon.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsEnumerable(this PropertyInfo info)
        {
            return info != null && info.PropertyType.IsEnumerable();
        }
    }
}