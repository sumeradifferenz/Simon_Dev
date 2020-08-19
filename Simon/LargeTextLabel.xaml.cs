using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Simon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LargeTextLabel : ContentView
    {
        public LargeTextLabel()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ExpandedProperty = BindableProperty.Create(
                        nameof(LargeTextLabel),
            typeof(bool),
            typeof(ContentView),
            false,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null && bindable is LargeTextLabel control)
                {
                    var actualNewValue = (bool)newValue;
                    control.SmallLabel.IsVisible = !actualNewValue;
                    control.FullLabel.IsVisible = actualNewValue;
                    control.ExpandContractButton.Text = actualNewValue ? "less" : "more";
                }
            });

        public bool Expanded { get; set; }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                        nameof(LargeTextLabel),
            typeof(string),
            typeof(ContentView),
            default(string),
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null && bindable is LargeTextLabel control)
                {
                    var actualNewValue = (string)newValue;
                    control.SmallLabel.Text = actualNewValue;
                    control.FullLabel.Text = actualNewValue;
                }
            });

        public string Text { get; set; }


        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
                        nameof(LargeTextLabel),
            typeof(ICommand),
            typeof(ContentView),
            default(Command),
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null && bindable is LargeTextLabel control)
                {
                    var actualNewValue = (ICommand)newValue;
                    control.ExpandContractButton.Command = actualNewValue;
                }
            });

        public ICommand Command { get; set; }
    }
}