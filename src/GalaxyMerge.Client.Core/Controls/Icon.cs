using System.Windows;
using System.Windows.Controls;

namespace GalaxyMerge.Client.Core.Controls
{
    public class Icon : Control
    {
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon),
                new FrameworkPropertyMetadata(typeof(Icon)));
            
            IsTabStopProperty.OverrideMetadata(typeof(Icon), 
                new FrameworkPropertyMetadata(false));
        }
    }
}