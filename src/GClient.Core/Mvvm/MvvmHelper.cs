using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GClient.Core.Mvvm
{
    public static class MvvmHelper
    {
        public static T GetImplementedType<T>(object obj, bool checkDataContext = true, bool avoidInheritedContext = false)
        {
            if (obj is T implementedType)
                return implementedType;

            if (!checkDataContext) return default;

            if (obj is not FrameworkElement {DataContext: T implementedContext} frameworkElement) return default;

            if (!avoidInheritedContext) return implementedContext;

            if (frameworkElement.Parent is not FrameworkElement frameworkElementParent) return implementedContext;
            if (frameworkElementParent is not T implementedParentContext) return implementedContext;
            return ReferenceEquals(implementedParentContext, implementedContext) ? default : implementedContext;
        }

        public static T GetImplementedAttribute<T>(object obj, bool checkDataContext = true)
            where T : Attribute
        {
            var attribute = GetCustomAttributes<T>(obj.GetType()).FirstOrDefault();
            if (attribute != null)
                return attribute;
            
            if (!checkDataContext) return default;

            if (obj is not FrameworkElement frameworkElement) return default;

            return frameworkElement.DataContext != null
                ? GetCustomAttributes<T>(frameworkElement.DataContext.GetType()).FirstOrDefault()
                : default;
        }

        public static IEnumerable<T> GetCustomAttributes<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }
    }
}