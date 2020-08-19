using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.ServiceHandler;
using Simon.Views;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private ObservableCollection<UserListModel> _userListItems = new ObservableCollection<UserListModel>();
        private ObservableCollection<UserListModel> AllUsers = new ObservableCollection<UserListModel>();

        HttpClient _httpClient;
        bool IsShowPass = false;
        
        public LoginViewModel()
        {
            HeaderTitle = Constants.SelectUsernameText;
            HeaderLeftImage = "back_arrow.png";
        }

        //private st1ring username;
        //public string UserName
        //{
        //    get { return username; }
        //    set
        //    {
        //        username = value;
        //        PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
        //    }
        //}

        private string password { get; set; }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        //public Command LoginCommand
        //{
        //    get
        //    {
        //        return new Command(Login);
        //    }
        //}

        public ObservableCollection<UserListModel> UserListItems
        {
            get { return _userListItems; }
            set
            {
                _userListItems = value;
                OnPropertyChanged(nameof(UserListItems));
            }
        }

        private UserListModel _selectedUser;
        public UserListModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                SetProperty(ref _selectedUser, value);
                //put here your code  
                //CityText = "City : " + _selectedCity.Value;
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetProperty(ref _Name, value);
            }
        }

        public ImageSource _HideShowPass { get; set; } = "view_pass.png";
        public ImageSource HideShowPass
        {
            get { return _HideShowPass; }
            set
            {
                _HideShowPass = value;
                OnPropertyChanged("HideShowPass");
            }
        }

        public string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                // _searchText = value;
                SetProperty(ref _searchText, value);
                SearchUser(value);
            }
        }

        private bool _isNodataFound { get; set; } = false;
        public bool IsNodataFound
        {
            get { return _isNodataFound; }
            set
            {
                _isNodataFound = value;
                OnPropertyChanged(nameof(IsNodataFound));
            }
        }

        //public async Task FetchUserData()
        //{
        //    using (HttpClient hc = new HttpClient())
        //    {
        //        try
        //        {
        //            IsBusy = true;
        //            var jsonString = await hc.GetStringAsync(Config.USER_LIST_API);
        //            UserListItems = UserListModel.FromJson(jsonString);
        //        }
        //        finally
        //        {
        //            IsBusy = false;
        //        }
        //    }
        //}

        //private void Login()
        //{
        //    //null or empty field validation, check weather email and password is null or empty  
        //    if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
        //        App.Current.MainPage.DisplayAlert("Empty Values", "Please enter Username and Password", "OK");
        //    else
        //    {
        //        //if (Email == "abc@gmail.com" && Password == "1234")
        //        //{
        //        //    App.Current.MainPage.DisplayAlert("Login Success", "", "Ok");
        //        //    //Navigate to Wellcom page after successfully login  
        //        //    App.Current.MainPage.Navigation.PushAsync(new WelcomPage());
        //        //}
        //        //else
        //            //App.Current.MainPage.DisplayAlert("Login Fail", "Please enter correct Email and Password", "OK");
        //    }
        //}

        public async void GetData()
        {
            try
            {
                await ShowLoader();
                App.buttonClick = 0;
                //Check network status   
                if (NetworkCheck.IsInternet())
                {
                    _httpClient = new HttpClient();
                    var response = await _httpClient.GetAsync(Config.USER_LIST_API);

                    await ClosePopup();

                    var respStr = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(respStr) && !string.IsNullOrWhiteSpace(respStr))
                    {
                        var data = JsonConvert.DeserializeObject<ObservableCollection<UserListModel>>(respStr);

                        if (data.Count > 0)
                        {
                            IsNodataFound = false;
                            UserListItems = data;
                            AllUsers = new ObservableCollection<UserListModel>(data.ToList());

                            if (!string.IsNullOrEmpty(SearchText))
                            {
                                SearchUser(SearchText);
                            }
                        }
                        else
                        {
                            IsNodataFound = true;
                        }
                    }
                }
                else
                {
                    await ClosePopup();
                    await ShowAlert("Simon", "No network is available.");
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                ShowExceptionAlert(ex);
            }
        }

        #region Command

        public ICommand SelectUsername_ClickCommand { get { return new Command(SelectUsername_Click); } }
        private async void SelectUsername_Click()
        {
            try
            {
                if (App.buttonClick == 0)
                {
                    App.buttonClick++;
                    this.ScreenTitle = Constants.SelectUsernameText;
                    await Application.Current.MainPage.Navigation.PushAsync(new SelectUsernamePage());
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand UserNameclicked { get { return new Command<UserListModel>(UserName_Click); } }
        private async void UserName_Click(UserListModel data)
        {
            try
            {
                var IsAnyUserNameSelected = UserListItems.Where(x => x.Radiobtnimg == "radio_select.png").ToList();
                if (IsAnyUserNameSelected.Count > 0 && IsAnyUserNameSelected != null)
                {
                    foreach (var item in IsAnyUserNameSelected)
                    {
                        item.Radiobtnimg = "radio_unselect.png";
                        item.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                    }
                }

                if (data != null)
                {
                    if (data.Radiobtnimg == "radio_unselect.png")
                    {
                        data.Radiobtnimg = "radio_select.png";
                        data.NamelblStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
                    }
                    else
                    {
                        data.Radiobtnimg = "radio_unselect.png";
                        data.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                    }

                    SelectedUser = data;
                    App.SelectedUserData = SelectedUser;
                    if (!string.IsNullOrEmpty(SelectedUser.name))
                    {
                        Name = SelectedUser.name;
                        App.SelectedUserData.name = Name;
                    }
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand LoginCommand { get { return new Command(DoLogin); } }
        private async void DoLogin()
        {
            try
            {
                if (App.SelectedUserData == null)
                {
                    await App.Current.MainPage.DisplayAlert("Simon", "Please select username", "OK");
                }
                else if (Password == null)
                {
                    await App.Current.MainPage.DisplayAlert("Simon", "Please enter password", "OK");
                }
                else
                {
                    await ShowLoader();
                    string userName = App.SelectedUserData.name;
                    string primaryRole = App.SelectedUserData.primaryRole;

                    var keyValues = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("UserName",userName),
                        new KeyValuePair<string, string>("Password",Password),
                        new KeyValuePair<string, string>("PrimaryRole",primaryRole),
                        new KeyValuePair<string, string>("UserProfile",null),
                    };

                    _httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, Config.LOGIN_API);

                    request.Content = new FormUrlEncodedContent(keyValues);
                    var response = await _httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    if (content == "" || Password.ToLower() != "cts")
                    {
                        await ClosePopup();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.Current.MainPage.DisplayAlert("Simon", "Password is incorrect", "OK");
                        });
                    }
                    else
                    {
                        Application.Current.Properties["NAME"] = App.SelectedUserData.firstName;
                        Application.Current.Properties["USERID"] = App.SelectedUserData.id;
                        Application.Current.Properties["PRIMARYROLE"] = App.SelectedUserData.primaryRole;
                        await Application.Current.SavePropertiesAsync();

                        await ClosePopup();

                        FooterNavigation(SessionService.BaseFooterItems[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand CleanSearchCommand { get { return new Command(CleanSearch_click); } }

        public ICommand SearchCommand { get { return new Command(SearchCommandExecuteAsync); } }

        #endregion

        private void SearchCommandExecuteAsync()
        {
            string value = SearchText;
            SearchUser(value);
        }

        private void SearchUser(string SearchString)
        {
            try
            {
                if (AllUsers.Count > 0)
                {
                    var tempUsersList = new ObservableCollection<UserListModel>();
                    if (!string.IsNullOrEmpty(SearchString) && !string.IsNullOrWhiteSpace(SearchString))
                    {
                        var Keyword = SearchString.ToLower().Trim();
                        if (Keyword.Length >= 1)
                        {
                            var searchedUser = AllUsers.Where(s => s.name.ToLower().Contains(Keyword)
                                        || s.firstName.ToString().Contains(Keyword.ToString()));

                            var tempList = searchedUser
                                        .OrderBy(item => item.name)
                                        .ToList();

                            if (tempList.Count > 0)
                            {
                                foreach (var item in tempList)
                                {
                                    tempUsersList.Add(item);
                                }
                                IsNodataFound = false;
                            }
                            else
                            {
                                IsNodataFound = true;
                            }
                        }
                        else
                        {
                            IsNodataFound = false;
                            foreach (var item in UserListItems)
                            {
                                tempUsersList.Add(item);
                            }
                        }

                        UserListItems = new ObservableCollection<UserListModel>(tempUsersList);
                    }
                    else
                    {
                        IsNodataFound = false;
                        UserListItems = new ObservableCollection<UserListModel>(AllUsers);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void CleanSearch_click()
        {
            SearchText = null;
        }

        string loginUserIdStr, refreshTokenstr;
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
                    await ShowAlert("Simon", "Username or password is incorrect");
                });
            }
            else
            {
                //Navigation.PushAsync(new DashboardPage());
            }
        }

    }
}

