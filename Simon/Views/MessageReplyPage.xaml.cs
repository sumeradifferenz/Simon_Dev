using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Simon.Views
{
    public partial class MessageReplyPage : ContentPage
    {
        List<string> country = new List<string>();
        UserListModel ObjAssignList = new UserListModel();
        DealMessageThreadList ObjThreadUsers = new DealMessageThreadList();
        string userId, nameUser, txtTopic, txtPartyName;
        IEnumerable<UserListModel> result;
        int threadId;
        public IEnumerable<string> threadUsersItemSource { get; set; }
        public List<messageUsers> dealthreadUsers { get; set; }
        private string selectedItem;
        IEnumerable<UserListModel> lstUsersListModel;
        ObservableCollection<messageUsers> data = new ObservableCollection<messageUsers>();
        MessageReplayViewModel thisVm = null;

        public ObservableCollection<messageUsers> threadUsers
        {
            get;
            set;
        }

        public MessageReplyPage()
        {
            InitializeComponent();
            //searchBarTxt.Focus();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            App.isFromAddParticipantPage = true;
            
            thisVm = new MessageReplayViewModel();
            this.BindingContext = thisVm;

            //if (Application.Current.Properties.ContainsKey("USERID"))
            //{
            //    userId = Convert.ToString(Application.Current.Properties["USERID"]);
            //}
            //if (Application.Current.Properties.ContainsKey("PARTYNAME"))
            //{
            //    txtPartyName = Convert.ToString(Application.Current.Properties["PARTYNAME"]);
            //    lblPartyName.Text = txtPartyName;
            //}
            //if (Application.Current.Properties.ContainsKey("TOPIC"))
            //{
            //    txtTopic = Convert.ToString(Application.Current.Properties["TOPIC"]);
            //    lblTopic.Text = txtTopic;
            //}

            if (NetworkCheck.IsInternet())
            {
                await thisVm.GetData();
                await thisVm.GetThreadUsersData();
            }
            else
            {
                await DisplayAlert("Simon", "No network is available.", "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            thisVm.Back();
            return true;
        }

        public void btnDeleteClicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                selectedItem = (sender as ImageButton).BindingContext.ToString();
            }
            var remove = threadUsers.Where(x => x.name == selectedItem).Single();
            threadUsers.Remove(remove);
            threadViewList.ItemsSource = threadUsers;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Has Backspace or Cancel has been pressed?
            if (e.NewTextValue == string.Empty)
            {
                // Yes, which one?
                if (e.OldTextValue.Length > 1)
                {
                    // Cancel has most probably been pressed
                    Debug.WriteLine("Cancel Pressed");
                    userList.IsVisible = false;
                }
                else
                {
                    // Backspace pressed on single character
                    // Cancel pressed on single character
                    Debug.WriteLine("Backspace or Cancel Pressed");
                    userList.IsVisible = false;
                }
            }
            else
            {
                var keyword = e.NewTextValue.ToLower();
                if (keyword.Length >= 1)
                {
                    var searchList = result.Where(x => x.name.ToLower().StartsWith(keyword));
                    lstUsersListModel = searchList.Select(t => new UserListModel()
                    {
                        name = t.name,
                        id = t.id
                    });
                    userList.ItemsSource = lstUsersListModel;
                    userList.SelectedItem = searchBarTxt.Text;
                    userList.IsVisible = true;
                }
                else
                {
                    userList.IsVisible = false;
                }
            }
        }

        private async void userList_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            UserListModel item = ((ListView)sender).SelectedItem as UserListModel;
            userList.IsVisible = true;
            if (!threadUsers.Any(p => p.userid_10 == item.id))
            {
                threadUsers.Add(new messageUsers { userid_10 = item.id, name = item.name });
                userList.IsVisible = false;
                lblEmptyList.IsVisible = false;
                threadViewList.ItemsSource = threadUsers;
            }
            else
            {
                userList.IsVisible = false;
                lblEmptyList.IsVisible = false;
            }
        }

    }
}
