using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GalaxyMerge.Client.Resources.Controls
{
    [ContentProperty("Content")]
    public partial class FormControl
    {
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(FormControl),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(FormControl),
                new PropertyMetadata(Orientation.Vertical));

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(FormControl),
                new PropertyMetadata("Label"));

        public static readonly DependencyProperty CaptionTextProperty =
            DependencyProperty.Register(nameof(CaptionText), typeof(string), typeof(FormControl),
                new PropertyMetadata("Caption"));

        public static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(FormControl),
                new PropertyMetadata(default(bool)));

        public string Error
        {
            get => (string) GetValue(ErrorProperty);
            set => SetValue(ErrorProperty, value);
        }

        public static readonly DependencyProperty ErrorProperty =
            DependencyProperty.Register(nameof(Error), typeof(string), typeof(FormControl),
                new PropertyMetadata(default(string)));

        public FormControl()
        {
            InitializeComponent();
            
            Validation.AddErrorHandler(this, OnValidationError);
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            HasError = e.Action == ValidationErrorEventAction.Added;
            Error = e.Error.ErrorContent.ToString();
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

        /*public string this[string columnName] =>
            Validation.GetHasError(this) ? Validation.GetErrors(this)[0].ErrorContent.ToString() : null;

        public string Error { get; private set; }*/
    }
}