using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Simon.Views.SubViews
{
    public partial class HeaderWithSingleImage : ContentView
    {
        public HeaderWithSingleImage()
        {
            InitializeComponent();

            TapGestureRecognizer LeftClickTGR = new TapGestureRecognizer();
            LeftClickTGR.Tapped += (object sender, EventArgs e) =>
            {
                if (LeftCommand != null)
                {
                    LeftCommand.Execute(this);
                }
            };
            //LeftImageView.GestureRecognizers.Add(LeftClickTGR);

            TapGestureRecognizer RightClickTGR = new TapGestureRecognizer();
            RightClickTGR.Tapped += (object sender, EventArgs e) =>
            {
                if (RightCommand != null)
                {
                    RightCommand.Execute(this);
                }
            };
            RightImageView.GestureRecognizers.Add(RightClickTGR);
        }

        public static BindableProperty LeftCommandProperty =
                 BindableProperty.Create(nameof(LeftCommand), typeof(Command), typeof(HeaderWithSingleImage), default(Command), BindingMode.TwoWay, propertyChanged: OnLeftCommandChanged);
        private static void OnLeftCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as HeaderWithSingleImage;
            picker.LeftCommand = (Command)newvalue;
            //picker.headertext.Text = newvalue;
        }

        public Command LeftCommand
        {
            get
            {
                return (Command)this.GetValue(LeftCommandProperty);
            }
            set
            {
                this.SetValue(LeftCommandProperty, value);
            }
        }


        public static BindableProperty RightCommandProperty =
                BindableProperty.Create(nameof(RightCommand), typeof(Command), typeof(HeaderWithSingleImage), default(Command), BindingMode.TwoWay, propertyChanged: OnRightCommandChanged);
        private static void OnRightCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as HeaderWithSingleImage;
            picker.RightCommand = (Xamarin.Forms.Command)newvalue;
        }

        public Command RightCommand
        {
            get
            {
                return (Command)this.GetValue(RightCommandProperty);
            }
            set
            {
                this.SetValue(RightCommandProperty, value);
            }
        }

        public static BindableProperty RightSecondCommandProperty =
                BindableProperty.Create(nameof(RightSecondCommand), typeof(Command), typeof(HeaderWithSingleImage), default(Command), BindingMode.TwoWay, propertyChanged: OnRightSecondCommandChanged);
        private static void OnRightSecondCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as HeaderWithSingleImage;
            picker.RightSecondCommand = (Xamarin.Forms.Command)newvalue;
        }

        public Command RightSecondCommand
        {
            get
            {
                return (Command)this.GetValue(RightSecondCommandProperty);
            }
            set
            {
                this.SetValue(RightSecondCommandProperty, value);
            }
        }

        public static BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(HeaderWithSingleImage), default(string), BindingMode.TwoWay, propertyChanged: OnHeaderTextChanged);
        private static void OnHeaderTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as HeaderWithSingleImage;
            string value = (string)newvalue;
            picker.HeaderText = value;
            picker.headertext.Text = value;
        }

        public string HeaderText
        {
            get
            {
                return (string)this.GetValue(HeaderTextProperty);
            }
            set
            {
                this.SetValue(HeaderTextProperty, value);
            }
        }

        public static BindableProperty LeftImageSourceProperty = BindableProperty.Create(nameof(LeftImageSource), typeof(ImageSource), typeof(HeaderWithSingleImage), default(ImageSource), BindingMode.TwoWay, propertyChanged: OnLeftImageSourceChanged);
        private static void OnLeftImageSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as HeaderWithSingleImage;
            picker.LeftImageSource = (Xamarin.Forms.ImageSource)newvalue;
            //picker.LeftImage.Source = (Xamarin.Forms.ImageSource)newvalue;
        }

        public ImageSource LeftImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(LeftImageSourceProperty);
            }
            set
            {
                this.SetValue(LeftImageSourceProperty, value);
            }
        }

        public static BindableProperty RightImageSourceProperty = BindableProperty.Create(nameof(RightImageSource), typeof(ImageSource), typeof(HeaderWithImageView), default(ImageSource), BindingMode.TwoWay, propertyChanged: OnRightImageSourceChanged);
        private static void OnRightImageSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as HeaderWithSingleImage;
            picker.RightImageSource = (Xamarin.Forms.ImageSource)newvalue;
            picker.RightImage.Source = (Xamarin.Forms.ImageSource)newvalue;
        }

        public ImageSource RightImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(RightImageSourceProperty);
            }
            set
            {
                this.SetValue(RightImageSourceProperty, value);
            }
        }
    }
}
