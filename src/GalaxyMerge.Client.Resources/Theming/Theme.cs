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
                    SetResource(ThemeResourceKey.PrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFBFBFC")));
                    SetResource(ThemeResourceKey.PrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF353B43")));
                    SetResource(ThemeResourceKey.PrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC7CED9")));
                    
                    //Generic
                    SetResource(ThemeResourceKey.PanelBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFEAEBEC")));
                    SetResource(ThemeResourceKey.IconForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF616A7A")));
                    SetResource(ThemeResourceKey.LabelForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    SetResource(ThemeResourceKey.CaptionForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    
                    //Control Generic
                    SetResource(ThemeResourceKey.ControlBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFFFFF")));
                    SetResource(ThemeResourceKey.ControlForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF242E40")));
                    SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF8A93A2")));
                    SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.6d);
                    SetResource(ThemeResourceKey.ControlDefaultForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB1B4B9")));
                    SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#BB9FAFCB")));
                    SetResource(ThemeResourceKey.ControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFDDE4EB")));
                    SetResource(ThemeResourceKey.ControlSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ControlSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFBFBFC")));

                    //Control Specific
                    //Buttons
                    SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF2F7FF")));
                    SetResource(ThemeResourceKey.ButtonPrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF4E6895")));
                    SetResource(ThemeResourceKey.ButtonPrimaryActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF6282BB")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFACB3BE")));
                    SetResource(ThemeResourceKey.ButtonSecondaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEEF3FF")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF808791")));
                    SetResource(ThemeResourceKey.ButtonSecondaryActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF99A0AA")));
                    SetResource(ThemeResourceKey.ButtonTernaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFE58585")));
                    SetResource(ThemeResourceKey.ButtonTernaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFFFF9F9")));
                    SetResource(ThemeResourceKey.ButtonTernaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA25A5A")));
                    SetResource(ThemeResourceKey.ButtonTernaryActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFC57171")));
                    SetResource(ThemeResourceKey.ButtonTransparentBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFC0C1C8")));
                    SetResource(ThemeResourceKey.ButtonTransparentActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFD3D6DB")));
                    //Check Box
                    SetResource(ThemeResourceKey.CheckBoxChecked.ToString(), new SolidColorBrush(ColorFromHex("#FF7398D8")));
                    SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF1F6FF")));
                    //ScrollBar
                    SetResource(ThemeResourceKey.ScrollBarThumbBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF899DB1")));
                    //Group Box
                    SetResource(ThemeResourceKey.GroupBoxHeaderBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
                    SetResource(ThemeResourceKey.GroupBoxHeaderForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFA53289")));
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
                    SetResource(ThemeResourceKey.LabelForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF898C95")));
                    SetResource(ThemeResourceKey.CaptionForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF5C6067")));
                    
                    //Control Generic
                    SetResource(ThemeResourceKey.ControlBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF15191C")));
                    SetResource(ThemeResourceKey.ControlForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFE1E6EE")));
                    SetResource(ThemeResourceKey.ControlBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF626A71")));
                    SetResource(ThemeResourceKey.ControlDisabledOpacity.ToString(), 0.6d);
                    SetResource(ThemeResourceKey.ControlDefaultForeground.ToString(), new SolidColorBrush(ColorFromHex("#FF767A81")));
                    SetResource(ThemeResourceKey.ControlFocusBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF68B1FB")));
                    SetResource(ThemeResourceKey.ControlHighlightBackground.ToString(), new SolidColorBrush(ColorFromHex("#BB377AF1")));
                    SetResource(ThemeResourceKey.ControlMouseOverBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF49535B")));
                    SetResource(ThemeResourceKey.ControlSelectedBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF48A0F8")));
                    SetResource(ThemeResourceKey.ControlSelectedForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFE3EAF8")));

                    //Control Specific
                    //Buttons
                    SetResource(ThemeResourceKey.ButtonPrimaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF3D83C8")));
                    SetResource(ThemeResourceKey.ButtonPrimaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFEEF1F5")));
                    SetResource(ThemeResourceKey.ButtonPrimaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF3C87D1")));
                    SetResource(ThemeResourceKey.ButtonPrimaryActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF2E6397")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF51575A")));
                    SetResource(ThemeResourceKey.ButtonSecondaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFB8BDBF")));
                    SetResource(ThemeResourceKey.ButtonSecondaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF676B6E")));
                    SetResource(ThemeResourceKey.ButtonSecondaryActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF35383A")));
                    SetResource(ThemeResourceKey.ButtonTernaryBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFDF5555")));
                    SetResource(ThemeResourceKey.ButtonTernaryForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF8F2F2")));
                    SetResource(ThemeResourceKey.ButtonTernaryBorder.ToString(), new SolidColorBrush(ColorFromHex("#FFCE5C5C")));
                    SetResource(ThemeResourceKey.ButtonTernaryActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FFA83F3F")));
                    SetResource(ThemeResourceKey.ButtonTransparentBorder.ToString(), new SolidColorBrush(ColorFromHex("#FF1D1E20")));
                    SetResource(ThemeResourceKey.ButtonTransparentActiveBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF2B2C2D")));
                    //Check Box
                    SetResource(ThemeResourceKey.CheckBoxChecked.ToString(), new SolidColorBrush(ColorFromHex("#FF3D83C8")));
                    SetResource(ThemeResourceKey.GlyphForeground.ToString(), new SolidColorBrush(ColorFromHex("#FFF1F6FF")));
                    //ScrollBar
                    SetResource(ThemeResourceKey.ScrollBarThumbBackground.ToString(), new SolidColorBrush(ColorFromHex("#FF667E92")));
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