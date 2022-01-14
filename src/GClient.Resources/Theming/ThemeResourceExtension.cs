using System;
using System.Windows;

namespace GClient.Resources.Theming
{
    public sealed class ThemeResourceExtension : DynamicResourceExtension
    {
        public ThemeResourceExtension()
        {
        }
        
        public ThemeResourceExtension(ThemeResourceKey resourceKey) : base(resourceKey)
        {
        }
        
        public new ThemeResourceKey ResourceKey
        {
            get
            {
                Enum.TryParse(base.ResourceKey.ToString(), out ThemeResourceKey resourceKey);
                return resourceKey;
            }
            set => base.ResourceKey = value.ToString();
        }
    }
}