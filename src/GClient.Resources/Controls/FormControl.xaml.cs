using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GClient.Resources.Controls
{
    [ContentProperty("Content")]
    public partial class FormControl
    {
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(FormControl),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(FormControl),
                new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(FormControl),
                new PropertyMetadata(default(string)));

        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register(nameof(LabelWidth), typeof(double), typeof(FormControl),
                new PropertyMetadata(default(double), OnLabelWidthChanged));

        public static readonly DependencyProperty CaptionTextProperty =
            DependencyProperty.Register(nameof(CaptionText), typeof(string), typeof(FormControl),
                new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(FormControl),
                new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(FormControl),
                new PropertyMetadata(default(string)));

        public FormControl()
        {
            InitializeComponent();
            
            Validation.AddErrorHandler(this, OnValidationError);
        }

        public new object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public string LabelText
        {
            get => (string) GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public double LabelWidth
        {
            get => (double) GetValue(LabelWidthProperty);
            set => SetValue(LabelWidthProperty, value);
        }

        public string CaptionText
        {
            get => (string) GetValue(CaptionTextProperty);
            set => SetValue(CaptionTextProperty, value);
        }

        public bool HasError
        {
            get => (bool) GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        public string ErrorMessage
        {
            get => (string) GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            HasError = e.Action == ValidationErrorEventAction.Added;
            ErrorMessage = e.Error.ErrorContent.ToString();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UserControl control) return;
            
            var formLabel = control.FindName("FormLabel");
            if (formLabel is not Label label) return;
            
            var orientation = (Orientation) e.NewValue;

            Grid.SetRow(label, orientation == Orientation.Horizontal ? 1 : 0);
            Grid.SetColumn(label, orientation == Orientation.Horizontal ? 0 : 1);
        }

        private static void OnLabelWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FormControl formControl) return;

            var target = formControl.FindName("Column1");
            if (target is not ColumnDefinition columnDefinition) return;

            if (formControl.Orientation == Orientation.Horizontal && formControl.LabelWidth > 0)
            {
                columnDefinition.Width = new GridLength(formControl.LabelWidth);
                return;
            }
            
            columnDefinition.Width = GridLength.Auto;
        }
    }
}