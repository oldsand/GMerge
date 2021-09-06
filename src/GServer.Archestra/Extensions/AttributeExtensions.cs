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
        public static string TryGetUnits(this IAttribute attribute)
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
        
        public static object GetValue(this IAttribute attribute)
        {
            return attribute.value.GetValue<object>();
        }

        public static T GetValue<T>(this IAttribute attribute)
        {
            return attribute.value.GetValue<T>();
        }
        
        public static void SetValue<T>(this IAttribute attribute, T value)
        {
            attribute.value.Clone(out var mxValue);
            mxValue.SetValue(value);
            attribute.SetValue(mxValue);
        }

        public static void Configure(this IAttribute attribute, ArchestraAttribute source, 
            string description = null, string units = null)
        {
            attribute.SetValue(source.Value);
            attribute.SetSecurityClassification(source.Security.ToMx());
            attribute.SetLocked(source.Locked.ToMx());
            
            if (!string.IsNullOrEmpty(description))
                attribute.Description = description;

            if (!string.IsNullOrEmpty(units))
                attribute.EngUnits = units;
        }

        public static ArchestraAttribute Map(this IAttribute attribute)
        {
            return new ArchestraAttribute(attribute.Name, 
                attribute.DataType.ToPrimitive(),
                attribute.AttributeCategory.ToPrimitive(),
                attribute.SecurityClassification.ToPrimitive(),
                attribute.Locked.ToPrimitive(),
                attribute.GetValue<object>(),
                attribute.UpperBoundDim1);
        }
    }
}