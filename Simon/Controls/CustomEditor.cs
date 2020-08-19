using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Simon.Controls
{
    public class CustomEditor : Editor
    {
       
        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
           propertyName: "ReturnType", returnType: typeof(ReturnType), declaringType: typeof(CustomEditor), defaultValue: ReturnType.Default);

        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
           propertyName: "ReturnCommand", returnType: typeof(ICommand), declaringType: typeof(CustomEditor), defaultValue: null);

        public ICommand ReturnCommand
        {
            get { return (ICommand)GetValue(ReturnCommandProperty); }
            set { SetValue(ReturnCommandProperty, value); }
        }

        public static BindableProperty IsExpandableProperty
        = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(CustomEditor), false);

        public bool IsExpandable
        {
            get { return (bool)GetValue(IsExpandableProperty); }
            set { SetValue(IsExpandableProperty, value); }
        }

        public CustomEditor()
        {
            //this.TextChanged += (sender, e) =>
            //{
            //    this.InvalidateMeasure();
            //};

            TextChanged += OnTextChanged;
        }

        ~CustomEditor()
        {
            TextChanged -= OnTextChanged;
        }

        protected void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable)
                InvalidateMeasure();
        }

        public static readonly BindableProperty IsCustomFocusedProperty =
            BindableProperty.Create("IsCustomFocused", typeof(bool), typeof(CustomEditor), false);

        public bool IsCustomFocused
        {
            get
            {
                return (bool)GetValue(IsCustomFocusedProperty);
            }
            set
            {
                SetValue(IsCustomFocusedProperty, value);
            }
        }
    }
}

