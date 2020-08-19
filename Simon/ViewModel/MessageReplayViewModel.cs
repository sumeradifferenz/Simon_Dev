using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.Views.Popups;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class MessageReplayViewModel : BaseViewModel
    {
        private ObservableCollection<DealMessageThreadList> _messageList = new ObservableCollection<DealMessageThreadList>();
        public List<messageUsers> DealthreadUsers = new List<messageUsers>();
        private ObservableCollection<messageUsers> _ThreadUsers = new ObservableCollection<messageUsers>();
        public ObservableCollection<MessageThread> _MessageThreadUsers = new ObservableCollection<MessageThread>();
        private ObservableCollection<UserListModel> _UserLists;
        private ObservableCollection<UserListModel> SearhUsersList = new ObservableCollection<UserListModel>();
        string username, userId;
        string threadId;

        HttpClient httpClient;

        public ICommand SendMessageCommand { get; set; }
        //public ICommand DeleteUserCommand { get; set; }

        public MessageReplayViewModel()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("USERID"))
                {
                    userId = (string)Application.Current.Properties["USERID"];
                }
                if (Application.Current.Properties.ContainsKey("PARTYNAME"))
                {
                    LabelParty = Convert.ToString(Application.Current.Properties["PARTYNAME"]);
                }
                if (Application.Current.Properties.ContainsKey("TOPIC"))
                {
                    LabelTopic = Convert.ToString(Application.Current.Properties["TOPIC"]);
                }
                if (Application.Current.Properties.ContainsKey("THREADID"))
                {
                    threadId = Convert.ToString(Application.Current.Properties["THREADID"]);
                }

                HeaderTitle = LabelParty;
                HeaderLeftImage = "back_arrow.png";

                SendMessageCommand = new Command(() => SendMessageCommandExecute());
                //DeleteUserCommand = new Command(() => DeleteUser());
            }
            catch (Exception ex)
            {
                ShowExceptionAlert(ex);
            }
        }

        private string _labelParty;
        public string LabelParty
        {
            get { return _labelParty; }
            set { SetProperty(ref _labelParty, value); }
        }

        private string _labelTopic;
        public string LabelTopic
        {
            get { return _labelTopic; }
            set { SetProperty(ref _labelTopic, value); }
        }

        public string _searchText { get; set; }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    var keyword = value.ToLower();
                    if (keyword.Length >= 1)
                    {
                        var searchList = SearhUsersList.Where(x => x.name.ToLower().StartsWith(keyword)).ToList();
                        var searchResult = searchList.Select(t => new UserListModel()
                        {
                            name = t.name,
                            id = t.id
                        });
                        UsersList.Clear();
                        UsersList = new ObservableCollection<UserListModel>(searchList);
                        IsUsersListVisible = true;
                    }
                    else
                    {
                        UsersList = new ObservableCollection<UserListModel>();
                        IsUsersListVisible = false;
                    }
                }
                else
                {
                    IsUsersListVisible = false;
                }
                OnPropertyChanged(SearchText);
            }
        }


        private bool _isStackViewVisible = true;
        public bool IsStackViewVisible
        {
            get { return _isStackViewVisible; }
            set { SetProperty(ref _isStackViewVisible, value); }
        }

        private bool _isCheckButtonVisible;
        public bool IsCheckButtonVisible
        {
            get { return _isCheckButtonVisible; }
            set { SetProperty(ref _isCheckButtonVisible, value); }
        }

        private bool _isCloseButtonVisible;
        public bool IsClosekButtonVisible
        {
            get { return _isCloseButtonVisible; }
            set { SetProperty(ref _isCloseButtonVisible, value); }
        }

        private bool _isSendLableVisible;
        public bool IsSendLableVisible
        {
            get { return _isSendLableVisible; }
            set { SetProperty(ref _isSendLableVisible, value); }
        }

        private bool _isUsersListVisible;
        public bool IsUsersListVisible
        {
            get { return _isUsersListVisible; }
            set { SetProperty(ref _isUsersListVisible, value); }
        }

        private bool _IsListdataAvailable;
        public bool IsListdataAvailable
        {
            get { return _IsListdataAvailable; }
            set { SetProperty(ref _IsListdataAvailable, value); }
        }
        
        private async void SendMessageCommandExecute()
        {
            //await SendMessage();
            //await SendThreadUsersAsync();
        }

        private bool _IsListEmpty { get; set; } = false;
        public bool IsListEmpty
        {
            get { return _IsListEmpty; }
            set
            {
                _IsListEmpty = value;
                OnPropertyChanged(nameof(IsListEmpty));
            }
        }

        public ObservableCollection<messageUsers> ThreadUsers
        {
            get { return _ThreadUsers; }
            set
            {
                _ThreadUsers = value;
                OnPropertyChanged(nameof(ThreadUsers));
            }
        }

        public ObservableCollection<MessageThread> MessageThreadUsers
        {
            get { return _MessageThreadUsers; }
            set
            {
                _MessageThreadUsers = value;
                OnPropertyChanged(nameof(MessageThreadUsers));
            }
        }

        public ObservableCollection<UserListModel> UsersList
        {
            get { return _UserLists; }
            set
            {
                _UserLists = value;
                OnPropertyChanged(nameof(UsersList));
            }
        }

        private UserListModel _selectedUser;
        public UserListModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        
        public ICommand UsersListTapCommnad { get { return new Command<UserListModel>(UsersListTap); } }
        private async void UsersListTap(UserListModel dealMessage)
        {
            try
            {
                httpClient = new HttpClient();
                IsUsersListVisible = true;
                if (!MessageThreadUsers.Any(p => p.id == dealMessage.id))
                {
                    MessageThreadUsers.Add(new MessageThread { id = dealMessage.id, name = dealMessage.name });

                    var content = new StringContent(JsonConvert.SerializeObject(MessageThreadUsers), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(Config.SYNC_PARTICIPANTS + threadId, content);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
                    {
                        await ClosePopup();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ShowAlert("Data Not Sent!!", string.Format("Response contained status code: {0}", response.StatusCode));
                        });
                    }
                    else
                    {
                        var responceContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(responceContent);
                    }

                    //Settings.MessageThreadUsersData = MessageThreadUsers;
                    SearchText = null;
                    IsUsersListVisible = false;
                    IsListEmpty = false;
                    IsListdataAvailable = true;
                    //threadViewList.ItemsSource = threadUsers;
                }
                else
                {
                    IsUsersListVisible = false;
                    IsListEmpty = false;
                    IsListdataAvailable = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionAlert(ex);
            }
        }
        public messageUsers threadUsersData { get; set; }
        public MessageThread messagethreadUsersData { get; set; }


        public ICommand DeleteUserCommand { get { return new Command<MessageThread>(DeleteUser); } }
        private async void DeleteUser(MessageThread user)
        {
            try
            {
                if (user != null) 
                {
                    messagethreadUsersData = user;
                }
                DeletePopup DeletePopupview = new DeletePopup();
                DeletePopupview.BindingContext = this;
                await ShowPopup(DeletePopupview);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ShowExceptionAlert(ex);
            }
        }

        public ICommand DeleteParticipantCommand { get { return new Command(DeleteParticipant_click); } }
        private async void DeleteParticipant_click()
        {
            try
            {
                if(messagethreadUsersData != null)
                {
                    var remove = MessageThreadUsers.Where(x => x.name == messagethreadUsersData.name).Single();
                    MessageThreadUsers.Remove(remove);

                    Settings.MessageThreadUsersData = MessageThreadUsers;

                    var content = new StringContent(JsonConvert.SerializeObject(MessageThreadUsers), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(Config.SYNC_PARTICIPANTS + threadId, content);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
                    {
                        await ClosePopup();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ShowAlert("Data Not Sent!!", string.Format("Response contained status code: {0}", response.StatusCode));
                        });
                    }
                    else
                    {
                        var responceContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(responceContent);
                    }

                    SearchText = null;
                    await ClosePopup();
                }
                else
                {
                    await ClosePopup();
                    await ShowAlert("Alert", "Data is not proper.");
                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ShowExceptionAlert(ex);
            }
        }

        public async Task GetData()
        {
            try
            {
                await ShowLoader();

                if (NetworkCheck.IsInternet())
                {
                    httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(Config.USER_LIST_API);
                    var respStr = await response.Content.ReadAsStringAsync();
                    if (respStr != "")
                    {
                        await ClosePopup();
                        var result = JsonConvert.DeserializeObject<IEnumerable<UserListModel>>(respStr);
                        var resultData = result.Select(t => new UserListModel()
                        {
                            name = t.name,
                            id = t.id
                        });

                        foreach (var Data in result)
                        {
                            username = Data.name;
                        }

                        UsersList = new ObservableCollection<UserListModel>(resultData.ToList());
                        SearhUsersList = new ObservableCollection<UserListModel>(resultData.ToList());
                    }
                }
                else
                {
                    await ClosePopup();
                    await ShowAlert("Alert", "No network is available.");
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                ShowExceptionAlert(ex);
            }
        }

        public async Task GetThreadUsersData()
        {
            try
            {
                await ShowLoader();
                if (NetworkCheck.IsInternet())
                {
                    httpClient = new System.Net.Http.HttpClient();
                    
                    IsBusy = true;

                    var response = await httpClient.GetStringAsync(Config.ADD_MESSAGES_THREAD_API + threadId);
                    if (!string.IsNullOrEmpty(response) && !string.IsNullOrWhiteSpace(response))
                    {
                        await ClosePopup();
                        var data = MessageThread.FromJson(response);

                        if (data.Count == 0)
                        {
                            IsListEmpty = true;
                            IsListdataAvailable = false;
                        }
                        else
                        {
                            IsListEmpty = false;
                            IsListdataAvailable = true;
                            MessageThreadUsers = data;
                            Settings.MessageThreadUsersData = MessageThreadUsers;
                        }
                    }
                }
                else
                {
                    await ClosePopup();
                    IsBusy = false;
                    await ShowAlert("Alert", "No network is available.");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionAlert(ex);
            }
            finally
            {
                await ClosePopup();
                IsBusy = false;
            }
        }

        public ICommand ClosePopup_Command { get { return new Command(ClosePopup_click); } }
        private async void ClosePopup_click()
        {
            await ClosePopup();
        }
    }
}

