using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace GClient.Core.Behaviors
{
    public static class Interactions
    {
        public static BehaviorsCollection GetBehaviors(DependencyObject obj)
        {
            return (BehaviorsCollection) obj.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject obj, BehaviorsCollection value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("Behaviors", typeof(BehaviorsCollection), typeof(Interactions),
                new UIPropertyMetadata(null, OnPropertyBehaviorsChanged));

        private static void OnPropertyBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is BehaviorsCollection)) return;
            
            var behaviors = Interaction.GetBehaviors(d);
            foreach (var behavior in (BehaviorsCollection) e.NewValue)
                behaviors.Add(behavior);
        }

        public static TriggersCollection GetTriggers(DependencyObject obj)
        {
            return (TriggersCollection) obj.GetValue(TriggersProperty);
        }

        public static void SetTriggers(DependencyObject obj, TriggersCollection value)
        {
            obj.SetValue(TriggersProperty, value);
        }

        public static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached("Triggers", typeof(TriggersCollection), typeof(Interactions),
                new UIPropertyMetadata(null, OnPropertyTriggersChanged));

        private static void OnPropertyTriggersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if (!(e.NewValue is TriggersCollection)) return;
            
            var triggers = Interaction.GetTriggers(d);
            foreach (var trigger in (TriggersCollection) e.NewValue)
                triggers.Add(trigger);
        }
    }
}