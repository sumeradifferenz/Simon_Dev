using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Simon.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Simon.Views.Popups
{
    public partial class DealSortByPopup : PopupPage
    {
        private DealViewModel vm = null;

        public DealSortByPopup()
        {
            InitializeComponent();
            vm = new DealViewModel();
            this.BindingContext = vm;

            if (Device.RuntimePlatform == Device.Android)
            {
                SortPopupStackVw.Padding = new Thickness(15, 120, 15, 120);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                if (mainDisplayInfo.Height <= 1334)
                {
                    // iPhone 6, 7, 8
                    SortPopupStackVw.Padding = new Thickness(15, 140, 15, 140);
                }
                else
                {
                    //iPhone X, XS, XR, 11
                    SortPopupStackVw.Padding = new Thickness(15, 200, 15, 200);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var thisVm = this.BindingContext as DealViewModel;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
