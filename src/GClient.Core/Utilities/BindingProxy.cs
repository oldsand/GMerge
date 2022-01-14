using System.Windows;

namespace GClient.Core.Utilities
{
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object DataContext
        {
            get => GetValue(DataContextProperty);
            set => SetValue(DataContextProperty, value);
        }

        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register(nameof(DataContext), typeof(object), typeof(BindingProxy),
                new PropertyMetadata(default(object)));
    }
}