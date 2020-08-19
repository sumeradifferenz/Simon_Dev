using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;

namespace Simon.Views.SubViews
{
    public partial class FooterView : ContentView
    {
        public FooterView()
        {
            InitializeComponent();
            var version = DeviceInfo.VersionString;

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    if (version >= "11")
            //    {

            //    }
            //}
        }
    }
}
