using System;
using System.Collections.Generic;
using Simon.Helpers;
using Simon.ServiceHandler;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Simon.Views.SubViews
{
    public partial class SimonFooterView : ContentView
    {
        public SimonFooterView()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<App>(this, "PushNotificationRecieved", (sender) => {
                App.Current.Resources["MsgCount"] = Settings.MessageCount;
            });

            if (Device.RuntimePlatform == Device.Android)
            {
                MainStackLayout.HeightRequest = LayoutService.HeightWidth70;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                if (mainDisplayInfo.Height <= 1334)
                {
                    // iPhone 6, 7, 8
                    MainStackLayout.HeightRequest = LayoutService.HeightWidth85;
                }
                else
                {
                    //iPhone X, XS, XR, 11
                    MainStackLayout.HeightRequest = LayoutService.HeightWidth70;
                }
            }
        }
    }
}
