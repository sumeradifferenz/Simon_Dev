using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Views
{
    public partial class LoginPage : ContentPage
    {
        string strPrimaryRole, strFullName, struserid, loginUserIdStr, refreshTokenstr;

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(App.SelectedUserData!=null)
            {
                txtusername.Text = App.SelectedUserData.name;
            }
            else
            {
                txtusername.Text = Constants.SelectUsernamePlaceholdertxt;
            }
        }

        private void ShowPassword_Tapped(object sender, EventArgs e)
        {
            if (txtpassword.IsPassword == true)
            {
                txtpassword.IsPassword = false;
                imgPassword.Source = "view_pass.png";
            }
            else
            {
                txtpassword.IsPassword = true;
                imgPassword.Source = "hide_pass.png";
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
