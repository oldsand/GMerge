using System;
using System.Windows;
using System.Windows.Media;

namespace GalaxyMerge.Client.Core.Themes
{
    public sealed class Theme
    {
        [ThreadStatic]
        private static ResourceDictionary _resourceDictionary;

        internal static ResourceDictionary ResourceDictionary
        {
            get
            {
                if (_resourceDictionary != null)
                {
                    return _resourceDictionary;
                }

                _resourceDictionary = new ResourceDictionary();
                LoadThemeType(ThemeType.Light);
                return _resourceDictionary;
            }
        }
        public static ThemeType ThemeType { get; set; } = ThemeType.Dark;

        public static void LoadThemeType(ThemeType type)
        {
            ThemeType = type;

            switch (type)
            {
                case ThemeType.Light:
                    {
                        //Window
                        SetResource(ThemeResourceKey.WindowBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF606060")));
                        SetResource(ThemeResourceKey.WindowControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE1E1E1")));
                        SetResource(ThemeResourceKey.WindowHeaderBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF2F2F2")));
                        SetResource(ThemeResourceKey.WindowHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF707070")));
                        
                        //Primary
                        SetResource(ThemeResourceKey.PrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE8E8E8")));
                        SetResource(ThemeResourceKey.PrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF4C5767")));
                        SetResource(ThemeResourceKey.PrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC2C4C7")));
                        
                        //Content
                        SetResource(ThemeResourceKey.ContentBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE2E3E4")));
                        SetResource(ThemeResourceKey.ContentForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF4C5767")));
                        SetResource(ThemeResourceKey.CaptionForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF87888D")));
                        
                        //Basic Control
                        SetResource(ThemeResourceKey.ControlBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF5F5F6")));
                        SetResource(ThemeResourceKey.ControlForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF353D4A")));
                        SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA2A9B5")));
                        SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#BB3EA2F1")));
                        SetResource(ThemeResourceKey.ControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF9F9F9")));
                        SetResource(ThemeResourceKey.ControlPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF9F9F9")));
                        SetResource(ThemeResourceKey.ControlPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC8C9CE")));
                        SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.6d);
                        SetResource(ThemeResourceKey.ControlDefaultBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC12B68")));
                        SetResource(ThemeResourceKey.ControlMouseOverBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF3B8BDE")));
                        
                        //Buttons
                        SetResource(ThemeResourceKey.ButtonDefaultBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonDefaultForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonDefaultBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonDefaultPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonDefaultPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonPrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonPrimaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ButtonPrimaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));

                        //Iconography
                        SetResource(ThemeResourceKey.IconForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF959595")));
                        SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF646769")));
                        
                        //Group Box
                        SetResource(ThemeResourceKey.GroupBoxHeaderBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.GroupBoxHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        
                        //Items Controls
                        SetResource(ThemeResourceKey.ListMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#22D9EDFF")));
                        SetResource(ThemeResourceKey.ListMouseOverBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFD9EDFF")));
                        SetResource(ThemeResourceKey.ListSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ListSelectedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF7FB2F0")));
                        SetResource(ThemeResourceKey.ListSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ListItemMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ListItemSelectedActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFD1E6FF")));
                        SetResource(ThemeResourceKey.ListItemSelectedInactiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE8EAEE")));
                        SetResource(ThemeResourceKey.TabItemMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFCFCFC")));
                        SetResource(ThemeResourceKey.TabItemSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFD0D0D6")));
                        SetResource(ThemeResourceKey.TabItemSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF336DD1")));

                        break;
                    }
                case ThemeType.Dark:
                    {
                        SetResource(ThemeResourceKey.PrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.PrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF3F3F3F")));
                        SetResource(ThemeResourceKey.PrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFDFDFDF")));
                        SetResource(ThemeResourceKey.IconForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF959595")));
                        SetResource(ThemeResourceKey.ContentBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ContentForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF3F3F3F")));
                        SetResource(ThemeResourceKey.ControlForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF3F3F3F")));
                        SetResource(ThemeResourceKey.ControlBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFDBE0E4")));
                        SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF8192A1")));
                        SetResource(ThemeResourceKey.ControlContentBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#77833AB4")));
                        SetResource(ThemeResourceKey.ControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFEDEFF2")));
                        SetResource(ThemeResourceKey.ControlPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFBFC2C4")));
                        SetResource(ThemeResourceKey.ControlPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF3CC0FF")));
                        SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.4d);
                        SetResource(ThemeResourceKey.ControlDefaultBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC12B68")));
                        SetResource(ThemeResourceKey.ControlMouseOverBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF833AB4")));
                        SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF646769")));
                        SetResource(ThemeResourceKey.GroupBoxHeaderBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.GroupBoxHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.WindowBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF606060")));
                        SetResource(ThemeResourceKey.WindowControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE6E6E6")));
                        SetResource(ThemeResourceKey.WindowHeaderBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF2F2F2")));
                        SetResource(ThemeResourceKey.WindowHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF707070")));
                        SetResource(ThemeResourceKey.ListMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#22833AB4")));
                        SetResource(ThemeResourceKey.ListMouseOverBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.ListSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF8F8F8")));
                        SetResource(ThemeResourceKey.ListSelectedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                        SetResource(ThemeResourceKey.ListSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                        break;
                    }
            }
        }

        public static object GetResource(ThemeResourceKey resourceKey)
        {
            return ResourceDictionary.Contains(resourceKey.ToString()) ? ResourceDictionary[resourceKey.ToString()] : null;
        }

        private static void SetResource(object key, object resource)
        {
            ResourceDictionary[key] = resource;
        }

        private static Color ColorFromHex(string colorHex)
        {
            return (Color?)ColorConverter.ConvertFromString(colorHex) ?? Colors.Transparent;
        }
    }
}