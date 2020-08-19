using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Views.Popups
{
    public partial class DealsFilterPopup : PopupPage
    {
        private DealViewModel vm = null;

        public DealsFilterPopup()
        {
            InitializeComponent();
            vm = new DealViewModel();
            this.BindingContext = vm;
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
