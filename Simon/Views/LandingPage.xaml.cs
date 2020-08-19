using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Simon.Controls;
using Simon.Helpers;
using Simon.Models;
using Simon.ServiceHandler;
using Simon.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Application = Xamarin.Forms.Application;

namespace Simon.Views
{
    public partial class LandingPage : GradientColorStack
    {
        private LandingViewModel ViewModel = null;

        public LandingPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            App.ReadUnread = "null";
            App.OrderByText = Constants.LastPostDateText;
            App.SelectedTitle = string.Empty;

            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            

            if (Application.Current.Properties.ContainsKey("NAME"))
            {
                var name = Convert.ToString(Application.Current.Properties["NAME"]);
                txtName.Text = App.SelectedUserData.name;
            }
            //_headerList.Add(new LandingModel { Date = "Date", Borrower = "Borrower", Amount = "Amount" });
            //headerList.ItemsSource = _headerList;
            ViewModel = new LandingViewModel();
            
            this.BindingContext = ViewModel;

            if (NetworkCheck.IsInternet())
            {
                await ViewModel.FetchClosingData();
                //await ViewModel.FetchDecisionDueData();
            }
            else
            {
                await DisplayAlert("Simon", "No network is available.", "OK");
            }
        }

        private void onDealBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DealsPage());
        }
        private void onApproveBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssentMainPage());
        }
        private void onPortfolioBtnClicked(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new MessageThreadPage());
        }
        private void onMessagesBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MessagesPage());
        }
        private void onAlertBtnClicked(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new MessageMainPage());
        }

        void SwipeGestureRecognizer_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
             ViewModel.FooterNavigation(SessionService.BaseFooterItems[1]);
        }
    }
}
