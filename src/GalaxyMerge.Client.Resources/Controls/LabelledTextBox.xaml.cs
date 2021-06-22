using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GalaxyMerge.Client.Core.Extensions;

namespace GalaxyMerge.Client.Resources.Controls
{
    public partial class LabelledTextBox : IDataErrorInfo
    {
        public LabelledTextBox()
        {
            InitializeComponent();
            RootGird.DataContext = this;
        }

        private void ValidationErrorHandler(object sender, ValidationErrorEventArgs e)
        {
            HasErrors = e.Action == ValidationErrorEventAction.Added;
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LabelledTextBox),
                new PropertyMetadata(default(string)));

        public bool HasErrors
        {
            get => (bool) GetValue(HasErrorsProperty);
            set => SetValue(HasErrorsProperty, value);
        }

        public static readonly DependencyProperty HasErrorsProperty =
            DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(LabelledTextBox),
                new PropertyMetadata(default(bool)));

        public string LabelText
        {
            get => (string) GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(LabelledTextBox),
                new PropertyMetadata("Label"));

        public string CaptionText
        {
            get => (string) GetValue(CaptionTextProperty);
            set => SetValue(CaptionTextProperty, value);
        }

        public static readonly DependencyProperty CaptionTextProperty =
            DependencyProperty.Register(nameof(CaptionText), typeof(string), typeof(LabelledTextBox),
                new PropertyMetadata("Caption"));

        public Thickness LabelMargin
        {
            get => (Thickness) GetValue(LabelMarginProperty);
            set => SetValue(LabelMarginProperty, value);
        }

        public static readonly DependencyProperty LabelMarginProperty =
            DependencyProperty.Register(nameof(LabelMargin), typeof(Thickness), typeof(LabelledTextBox),
                new PropertyMetadata(new Thickness(0, 0, 0, 2)));

        public Thickness CaptionMargin
        {
            get => (Thickness) GetValue(CaptionMarginProperty);
            set => SetValue(CaptionMarginProperty, value);
        }

        public static readonly DependencyProperty CaptionMarginProperty =
            DependencyProperty.Register(nameof(CaptionMargin), typeof(Thickness), typeof(LabelledTextBox),
                new PropertyMetadata(new Thickness(0, 2, 0, 0)));

        public string this[string columnName] =>
            Validation.GetHasError(this) ? Validation.GetErrors(this)[0].ErrorContent.ToString() : null;

        public string Error => throw new NotImplementedException();
    }
}