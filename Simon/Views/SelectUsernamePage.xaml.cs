using System;
using System.Collections.Generic;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Views
{
    public partial class SelectUsernamePage : ContentPage
    {
        public SelectUsernamePage()
        {
            InitializeComponent();

            Donebtn.Clicked += Donebtn_clicked;
        }

        public void Donebtn_clicked(object sender,EventArgs e)
        {
            var thisVmContext = this.BindingContext as LoginViewModel;
            App.SelectedUserData = thisVmContext.SelectedUser;

            if(App.SelectedUserData == null)
            {
                DisplayAlert("Alert", "Please select User", "CANCEL");
            }
            else
            {
                Navigation.PopAsync();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var thisVm = this.BindingContext as LoginViewModel;
            thisVm.GetData();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var thisVmContext = this.BindingContext as LoginViewModel;
            App.SelectedUserData = thisVmContext.SelectedUser;
        }

    }
}
