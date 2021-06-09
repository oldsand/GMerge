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
                    SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF8A93A2")));
                    SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.6d);
                    SetResource(ThemeResourceKey.ControlDefaultForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#BB9FAFCB")));

                    //Control Specific
                    //Buttons
                    SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF2F7FF")));
                    SetResource(ThemeResourceKey.ButtonPrimaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFA0C3F9")));
                    SetResource(ThemeResourceKey.ButtonPrimaryPressedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF637CA7")));
                    SetResource(ThemeResourceKey.ButtonPrimaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF637CA7")));
                    SetResource(ThemeResourceKey.ButtonPrimaryMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF6282BB")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFCDD4DB")));
                    SetResource(ThemeResourceKey.ButtonSecondaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF83868E")));
                    SetResource(ThemeResourceKey.ButtonSecondaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFDCE7F2")));
                    SetResource(ThemeResourceKey.ButtonSecondaryPressedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF83868E")));
                    SetResource(ThemeResourceKey.ButtonSecondaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF83868E")));
                    SetResource(ThemeResourceKey.ButtonSecondaryMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFB4BAC1")));
                    SetResource(ThemeResourceKey.ButtonTernaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE58585")));
                    SetResource(ThemeResourceKey.ButtonTernaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFF9F9")));
                    SetResource(ThemeResourceKey.ButtonTernaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFF9AEAE")));
                    SetResource(ThemeResourceKey.ButtonTernaryPressedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF9B5A5A")));
                    SetResource(ThemeResourceKey.ButtonTernaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF9B5A5A")));
                    SetResource(ThemeResourceKey.ButtonTernaryMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFC57171")));
                    SetResource(ThemeResourceKey.ButtonTransparentPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFD3D6DB")));
                    SetResource(ThemeResourceKey.ButtonTransparentPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC0C1C8")));
                    SetResource(ThemeResourceKey.ButtonTransparentMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFD3D6DB")));
                    //Check Box
                    SetResource(ThemeResourceKey.CheckBoxChecked.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF1F6FF")));
                    //Group Box
                    SetResource(ThemeResourceKey.GroupBoxHeaderBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                    SetResource(ThemeResourceKey.GroupBoxHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                    
                    //Items Controls
                    SetResource(ThemeResourceKey.ListMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFDDE4EB")));
                    SetResource(ThemeResourceKey.ListMouseOverForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFBFBFC")));
                    SetResource(ThemeResourceKey.ListSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ListSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFBFBFC")));
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
                    SetResource(ThemeResourceKey.PrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF121517")));
                    
                    //Generic
                    SetResource(ThemeResourceKey.PanelBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF34383B")));
                    SetResource(ThemeResourceKey.IconForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF969DAB")));
                    SetResource(ThemeResourceKey.CaptionForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    
                    //Control Generic
                    SetResource(ThemeResourceKey.ControlBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF15191C")));
                    SetResource(ThemeResourceKey.ControlForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEAEFF9")));
                    SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF626A71")));
                    SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.6d);
                    SetResource(ThemeResourceKey.ControlDefaultForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF535860")));
                    SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF68B1FB")));
                    SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#BB377AF1")));

                    //Control Specific
                    //Buttons
                    SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF3D83C8")));
                    SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEEF1F5")));
                    SetResource(ThemeResourceKey.ButtonPrimaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF18324D")));
                    SetResource(ThemeResourceKey.ButtonPrimaryPressedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF4D9EEE")));
                    SetResource(ThemeResourceKey.ButtonPrimaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF4D9EEE")));
                    SetResource(ThemeResourceKey.ButtonPrimaryMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF2E6397")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF51575A")));
                    SetResource(ThemeResourceKey.ButtonSecondaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB8BDBF")));
                    SetResource(ThemeResourceKey.ButtonSecondaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF333739")));
                    SetResource(ThemeResourceKey.ButtonSecondaryPressedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF98A1A7")));
                    SetResource(ThemeResourceKey.ButtonSecondaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF98A1A7")));
                    SetResource(ThemeResourceKey.ButtonSecondaryMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF35383A")));
                    SetResource(ThemeResourceKey.ButtonTernaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFDF5555")));
                    SetResource(ThemeResourceKey.ButtonTernaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF8F2F2")));
                    SetResource(ThemeResourceKey.ButtonTernaryPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF6D2929")));
                    SetResource(ThemeResourceKey.ButtonTernaryPressedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEE5A5A")));
                    SetResource(ThemeResourceKey.ButtonTernaryPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFEE5A5A")));
                    SetResource(ThemeResourceKey.ButtonTernaryMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFA83F3F")));
                    SetResource(ThemeResourceKey.ButtonTransparentPressedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF2B2C2D")));
                    SetResource(ThemeResourceKey.ButtonTransparentPressedBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF1D1E20")));
                    SetResource(ThemeResourceKey.ButtonTransparentMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF2B2C2D")));
                    //Check Box
                    SetResource(ThemeResourceKey.CheckBoxChecked.ToString(), new SolidColorBrush(ColorFromHex("#FF3D83C8")));
                    SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF1F6FF")));
                    
                    //Items Controls
                    SetResource(ThemeResourceKey.ListMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF0F1317")));
                    SetResource(ThemeResourceKey.ListSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ListSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFE3EAF8")));
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