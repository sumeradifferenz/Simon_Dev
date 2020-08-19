using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Simon.Views
{
    public partial class AlertPage : ContentPage
    {
        public AlertPage()
        {
            InitializeComponent();
        }
        public Label LblTitle
        {
            get
            {
                return lblTitle;
            }
        }
        public Label LblText
        {
            get
            {
                return lblText;
            }
        }

        public Button Button1
        {
            get
            {
                return btn1;
            }
        }

        public Button Button2
        {
            get
            {
                return btn2;
            }
        }

    }
}
