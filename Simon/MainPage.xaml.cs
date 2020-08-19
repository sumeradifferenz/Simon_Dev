using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Interfaces;
using Simon.Models;
using Simon.ViewModel;
using Simon.Views;
using Xamarin.Forms;

namespace Simon
{
    public partial class MainPage : ContentPage
    {
        IEnumerable<UserListModel> result;
        string strPrimaryRole, strFullName, struserid, loginUserIdStr, refreshTokenstr;

        public MainPage()
        {
            InitializeComponent();
            GetJSON();
        }
        public async void GetJSON()
        {
            //Check network status   
            if (NetworkCheck.IsInternet())
            {
                var httpClient = new System.Net.Http.HttpClient();
                var response = await httpClient.GetAsync(Config.USER_LIST_API);
                var respStr = await response.Content.ReadAsStringAsync();
                if (respStr != "")
                {
                    result = JsonConvert.DeserializeObject<IEnumerable<UserListModel>>(respStr);
                }
                //Binding listview with server response    
                userListPicker.ItemsSource = (System.Collections.IList)result;
            }
            else
            {
                await DisplayAlert("Simon", "No network is available.", "OK");
            }
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                UserListModel selectedPickerItem = picker.SelectedItem as UserListModel;
                strPrimaryRole = selectedPickerItem.primaryRole;
                struserid = selectedPickerItem.id;
                strFullName = selectedPickerItem.firstName;
            }
        }

        public async void LoginAsync()
        {
            if (userListPicker.SelectedIndex >= 0)
            {

                string userName = userListPicker.Items[userListPicker.SelectedIndex];
                int selectedIndexPicker = userListPicker.SelectedIndex;
                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("UserName",userName),
                    new KeyValuePair<string, string>("Password",psw.Text),
                    new KeyValuePair<string, string>("PrimaryRole",strPrimaryRole),
                    new KeyValuePair<string, string>("UserProfile",null),
                };
                var request = new HttpRequestMessage(HttpMethod.Post, Config.LOGIN_API);

                request.Content = new FormUrlEncodedContent(keyValues);
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);

                if (content == "" || psw.Text != "cts")
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Simon", "Username or password is incorrect", "OK");

                    });
                }
                else if (psw.Text == "cts")
                {
                    Application.Current.Properties["NAME"] = strFullName;
                    Application.Current.Properties["USERID"] = struserid;
                    Application.Current.Properties["PRIMARYROLE"] = strPrimaryRole;
                    Application.Current.SavePropertiesAsync();
                    //Sending firebase token 
                    SaveUserRegistration();
                    //Navigation.PushAsync(new DashboardPage());
                    Application.Current.MainPage = new NavigationPage(new SimonTabbedPage());
                    //Navigation.PushAsync(new SimonTabbedPage());
                }
            }
            else
            {
                await DisplayAlert("Simon", "Please Select Username.", "OK");

            }
        }

        public async void SaveUserRegistration()
        {

            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                loginUserIdStr = Convert.ToString(Application.Current.Properties["USERID"]);
            }

            if (Application.Current.Properties.ContainsKey("REFRESHTOKEN"))
            {
                refreshTokenstr = Convert.ToString(Application.Current.Properties["REFRESHTOKEN"]);
            }
            var kvalues = new Dictionary<object, object>
            {
                {"userid",loginUserIdStr },
                {"registrationcode",refreshTokenstr}

            };
            var request = new HttpRequestMessage(HttpMethod.Post, Config.SAVE_REGISTRATION_API + loginUserIdStr);
            var client = new HttpClient();
            var content1 = new StringContent(JsonConvert.SerializeObject(kvalues), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Config.SAVE_REGISTRATION_API + loginUserIdStr, content1);
            var content = await response.Content.ReadAsStringAsync();
            // Debug.WriteLine(content);

            if (content == "")
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Simon", "Username or password is incorrect", "OK");

                });
            }
            else
            {

                //Navigation.PushAsync(new DashboardPage());
            }
        }

        public void OnLogInButtonClicked(object sender, EventArgs e)
        {
            LoginAsync();
        }
    }
}