using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Simon.Controls;
using Simon.Helpers;
using Simon.Models;
using Simon.ServiceHandler;
using Simon.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Simon.Views
{
    public partial class AssentMainPage : GradientColorStack
    {
        IEnumerable<ApprovalMainModel> ObjAssignList;
        string userId;
        private ApprovalViewModel ViewModel = null;

        public AssentMainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<object, ApprovalMainModel>(this, "ApprovalSortBorrowerUp", (sender, args) =>
            {
                assignListView.ScrollTo(args, ScrollToPosition.Start, true);
            });

            MessagingCenter.Subscribe<object, ApprovalMainModel>(this, "ApprovalSortBorrowerDown", (sender, args) =>
            {
                assignListView.ScrollTo(args, ScrollToPosition.Start, true);
            });

            MessagingCenter.Subscribe<object, ApprovalMainModel>(this, "ApprovalSortDateUp", (sender, args) =>
            {
                assignListView.ScrollTo(args, ScrollToPosition.Start, true);
            });

            MessagingCenter.Subscribe<object, ApprovalMainModel>(this, "ApprovalSortDateDown", (sender, args) =>
            {
                assignListView.ScrollTo(args, ScrollToPosition.Start, true);
            });

            MessagingCenter.Subscribe<object, ApprovalMainModel>(this, "ClearApprovalSort", (sender, args) =>
            {
                assignListView.ScrollTo(args, ScrollToPosition.Start, true);
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            App.ReadUnread = "null";
            App.OrderByText = Constants.LastPostDateText;
            App.SelectedTitle = string.Empty;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Xamarin.Forms.Application.Current.Properties["USERID"]);
            }
            assignListView.RefreshCommand = new Command(() =>
            {
                assignListView.IsRefreshing = false;
            });


            ViewModel = new ApprovalViewModel();
            this.BindingContext = ViewModel;

            if (NetworkCheck.IsInternet())
            {
                await ViewModel.GetApprovalData();
            }
            else
            {
                await DisplayAlert("Simon", "No network is available.", "OK");
            }
        }

        protected override void OnDisappearing() { }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void SwipeToRight(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            ViewModel.FooterNavigation(SessionService.BaseFooterItems[2]);
        }

        private async void btnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                assignListView.ItemsSource = ViewModel.ApprovalItems;
            }
            else
            {
                var Keyword = e.NewTextValue.ToLower();
                assignListView.ItemsSource = ViewModel.ApprovalItems.Where(x => x.reqname_10.ToLower().Contains(Keyword) || x.partyname_10.ToLower().Contains(Keyword) || x.productdesc_10.ToLower().Contains(Keyword));
            }
        }
    }
}
