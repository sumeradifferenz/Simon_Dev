using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Simon.Views.Popups
{
    public partial class DeletePopup : PopupPage
    {
        public DeletePopup()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                popupStackView.Padding = new Thickness(15, 120, 15, 120);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                if (mainDisplayInfo.Height <= 1334)
                {
                    // iPhone 6, 7, 8
                    popupStackView.Padding = new Thickness(20, 200, 20, 200);
                }
                else
                {
                    //iPhone X, XS, XR, 11
                    popupStackView.Padding = new Thickness(20, 270, 20, 270);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
