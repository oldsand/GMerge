using System;
using System.Windows;
using EventTrigger = Microsoft.Xaml.Behaviors.EventTrigger;

namespace GalaxyMerge.Client.Core.Behaviors
{
    public class HandlingEventTrigger : EventTrigger
    {
        protected override void OnEvent(EventArgs eventArgs)
        {
            if (eventArgs is RoutedEventArgs routedEventArgs)
                routedEventArgs.Handled = true;
            
            base.OnEvent(eventArgs);
        }
    }
}