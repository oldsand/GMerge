using System;
using System.Windows;
using System.Windows.Media;

namespace GalaxyMerge.Client.Resources.Theming
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
        
        public static ThemeType ThemeType { get; set; }

        public static void LoadThemeType(ThemeType type)
        {
            ThemeType = type;
            
            //Window
            SetResource(ThemeResourceKey.WindowHeaderBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF48505E")));
            SetResource(ThemeResourceKey.WindowHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB0C2E2")));
            SetResource(ThemeResourceKey.WindowBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF313640")));
            SetResource(ThemeResourceKey.WindowControlMouseOverBackgroundStandard.ToString(), new SolidColorBrush(ColorFromHex("#FF313640")));
            SetResource(ThemeResourceKey.WindowControlMouseOverBackgroundClose.ToString(), new SolidColorBrush(ColorFromHex("#FFD66F6F")));

            switch (type)
            {
                case ThemeType.Light:
                { 
                    //Primary
                    SetResource(ThemeResourceKey.PrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                    SetResource(ThemeResourceKey.PrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF353B43")));
                    SetResource(ThemeResourceKey.PrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFBCBFC4")));
                    
                    //Generic
                    SetResource(ThemeResourceKey.PanelBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF3F3F3")));
                    SetResource(ThemeResourceKey.IconForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF616A7A")));
                    SetResource(ThemeResourceKey.CaptionForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    
                    //Control Generic
                    SetResource(ThemeResourceKey.ControlBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFBFBFC")));
                    SetResource(ThemeResourceKey.ControlForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF242E40")));
                    SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF42607A")));
                    SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.6d);
                    SetResource(ThemeResourceKey.ControlDefaultForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#BB9FAFCB")));
                    SetResource(ThemeResourceKey.ControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF9F9F9")));
                    SetResource(ThemeResourceKey.ControlMouseOverBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                    SetResource(ThemeResourceKey.ControlPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF9F9F9")));
                    SetResource(ThemeResourceKey.ControlPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF3A414D")));

                    //Control Specific
                    //Buttons
                    SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEEF4FC")));
                    SetResource(ThemeResourceKey.ButtonPrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF6484BC")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFCDD2D8")));
                    SetResource(ThemeResourceKey.ButtonSecondaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF7D8084")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC0C3C9")));
                    SetResource(ThemeResourceKey.ButtonTernaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE58585")));
                    SetResource(ThemeResourceKey.ButtonTernaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFE9E5E5")));
                    SetResource(ThemeResourceKey.ButtonTernaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC77474")));
                    //Check Box
                    SetResource(ThemeResourceKey.CheckBoxChecked.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF1F6FF")));
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
                    //Primary
                    SetResource(ThemeResourceKey.PrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF222529")));
                    SetResource(ThemeResourceKey.PrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFC6D0DB")));
                    SetResource(ThemeResourceKey.PrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF1A1D20")));
                    
                    //Generic
                    SetResource(ThemeResourceKey.PanelBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF292D31")));
                    SetResource(ThemeResourceKey.IconForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF616A7A")));
                    SetResource(ThemeResourceKey.CaptionForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    
                    //Control Specific
                    //Buttons
                    SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEEF4FC")));
                    SetResource(ThemeResourceKey.ButtonPrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF6484BC")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFCDD2D8")));
                    SetResource(ThemeResourceKey.ButtonSecondaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF7D8084")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC0C3C9")));
                    SetResource(ThemeResourceKey.ButtonTernaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE58585")));
                    SetResource(ThemeResourceKey.ButtonTernaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFE9E5E5")));
                    SetResource(ThemeResourceKey.ButtonTernaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC77474")));
                    //Check Box
                    SetResource(ThemeResourceKey.CheckBoxChecked.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF1F6FF")));
                    //Group Box
                    SetResource(ThemeResourceKey.GroupBoxHeaderBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                    SetResource(ThemeResourceKey.GroupBoxHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
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