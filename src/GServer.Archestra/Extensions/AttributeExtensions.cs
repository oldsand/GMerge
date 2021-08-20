using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

namespace GServer.Archestra.Extensions
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Wraps get in try/catch because if an attribute has no EngUnits,
        /// IAttribute throws a COM exception when trying to obtain the value...
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>string</returns>
        public static string TryGetEngUnits(this IAttribute attribute)
        {
            try
            {
                return attribute.EngUnits;
            }
            catch (COMException)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Wraps get in try/catch because some attributes throw a COMException when trying to obtain the value...
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>bool</returns>
        public static bool TryGetCfgSetHandler(this IAttribute attribute)
        {
            try
            {
                return attribute.CfgSethandler;
            }
            catch (COMException)
            {
                return false;
            }
        }

        /// <summary>
        /// Wraps get in try/catch because some attributes throw a COMException when trying to obtain the value...
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>bool</returns>
        public static bool TryGetRtSetHandler(this IAttribute attribute)
        {
            try
            {
                return attribute.RtSethandler;
            }
            catch (COMException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Wraps get in try/catch because some attributes throw a COMException when trying to obtain the value...
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>bool</returns>
        public static bool TryGetHasBuffer(this IAttribute attribute)
        {
            try
            {
                return attribute.HasBuffer;
            }
            catch (COMException)
            {
                return false;
            }
        }

        public static T GetValue<T>(this IAttribute attribute)
        {
            return attribute.value.GetValue<T>();
        }
        
        public static void SetValue<T>(this IAttribute attribute, T value)
        {
            attribute.value.Clone(out var mxValue); 
            mxValue.SetValue(value, attribute.DataType.ToPrimitiveType());
            attribute.SetValue(mxValue);
        }

        public static ArchestraAttribute Map(this IAttribute attribute)
        {
            return attribute.MapInternal();
        }

        public static IEnumerable<ArchestraAttribute> Map(this IAttributes attributes)
        {
            foreach (IAttribute attribute in attributes)
                yield return attribute.MapInternal();
        }
        
        public static IEnumerable<ArchestraAttribute> ByDataType(this IAttributes attributes, DataType dataType)
        {
            foreach (IAttribute attribute in attributes)
                if (attribute.DataType == dataType.ToMxType())
                    yield return attribute.MapInternal();
        }
        
        public static IEnumerable<ArchestraAttribute> ByNameContains(this IAttributes attributes, string name)
        {
            foreach (IAttribute attribute in attributes)
                if (attribute.Name.Contains(name))
                    yield return attribute.MapInternal();
        }

        private static ArchestraAttribute MapInternal(this IAttribute attribute)
        {
            return new(attribute.Name, attribute.DataType.ToPrimitiveType())
            {
                Category = attribute.AttributeCategory.ToPrimitiveType(),
                Security = attribute.SecurityClassification.ToPrimitiveType(),
                Locked = attribute.Locked.ToPrimitiveType(),
                ArrayCount = attribute.UpperBoundDim1,
                Value = attribute.GetValue<object>()
            };
        }
    }
}