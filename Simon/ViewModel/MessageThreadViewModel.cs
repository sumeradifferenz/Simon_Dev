using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simon.Helpers;
using Simon.Models;
using Simon.Views;
using Simon.Views.Popups;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class MessageThreadViewModel : BaseViewModel
    {
        string userId, threadId, name;
        bool bookMarks, sendEnable;
        int id;
        string message;
        JObject jObject = null;
        private ObservableCollection<messages> _messageList = new ObservableCollection<messages>();
        private ObservableCollection<messageUsers> _threadList = new ObservableCollection<messageUsers>();
        private ObservableCollection<messages> AllItems = new ObservableCollection<messages>();
        private string _plainContent;
        public ICommand PersonDetailsCommand { get; set; }
        public ICommand ReplayCommand { get; set; }
        public ICommand LoadMoreCommand { get; private set; }

        private bool _isLoadingInfinite = false;
        private int _totalRecords = 0;
        private bool _isLoadingInfiniteEnabled = false;
        private int _CurrentPage = 1;
        private int _LastPage = 0;
        private bool _isTeamLoading = false;

        private ObservableCollection<messageUsers> _messageUserList = new ObservableCollection<messageUsers>();
        public ObservableCollection<messageUsers> messageUserList
        {
            get { return _messageUserList; }
            set
            {
                _messageUserList = value;
                OnPropertyChanged(nameof(messageUserList));
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

        public string _TypedMessage { get; set; }
        public string TypedMessage
        {
            get { return _TypedMessage; }
            set
            {
                _TypedMessage = value;
                OnPropertyChanged(TypedMessage);     
            }
        }

        public string _ParticipantName { get; set; }
        public string ParticipantName
        {
            get { return _ParticipantName; }
            set
            {
                _ParticipantName = value;
                OnPropertyChanged(ParticipantName);
            }
        }

        private bool _IsDataNotAvailable { get; set; } = false;
        public bool IsDataNotAvailable
        {
            get { return _IsDataNotAvailable; }
            set
            {
                _IsDataNotAvailable = value;
                OnPropertyChanged(nameof(IsDataNotAvailable));
            }
        }

        private bool _IsListDataAvailable { get; set; } = false;
        public bool IsListDataAvailable
        {
            get { return _IsListDataAvailable; }
            set
            {
                _IsListDataAvailable = value;
                OnPropertyChanged(nameof(IsListDataAvailable));
            }
        }

        private DateTime _msgCreatedDate;
        public DateTime MsgCreatedDate
        {
            get { return _msgCreatedDate; }
            set
            {
                _msgCreatedDate = value;
                OnPropertyChanged(nameof(MsgCreatedDate));
            }
        }

        public bool IsLoadingInfinite
        {
            get { return _isLoadingInfinite; }
            set { SetProperty(ref _isLoadingInfinite, value); }
        }
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { SetProperty(ref _totalRecords, value); }
        }
        public bool IsLoadingInfiniteEnabled
        {
            get { return _isLoadingInfiniteEnabled; }
            set { SetProperty(ref _isLoadingInfiniteEnabled, value); }
        }

        HttpClient httpClient;

        public ICommand SendMessageCommand { get; set; }

        public MessageThreadViewModel()
        {
            if (Application.Current.Properties.ContainsKey("PARTYNAME"))
            {
                LabelParty = Convert.ToString(Application.Current.Properties["PARTYNAME"]);
            }

            if (Application.Current.Properties.ContainsKey("TOPIC"))
            {
                LabelTopic = Convert.ToString(Application.Current.Properties["TOPIC"]);
            }

            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Application.Current.Properties["USERID"]);
            }
            if (Application.Current.Properties.ContainsKey("THREADID"))
            {
                threadId = Convert.ToString(Application.Current.Properties["THREADID"]);
            }
            if (Application.Current.Properties.ContainsKey("ID"))
            {
                id = Convert.ToInt16(Application.Current.Properties["ID"]);
            }
            if (Application.Current.Properties.ContainsKey("NAME"))
            {
                name = Convert.ToString(Application.Current.Properties["NAME"]);
            }

            HeaderTitle = LabelParty;
            HeaderLeftImage = "back_arrow.png";

            PersonDetailsCommand = new Command(() => PersonDetailsCommandExecute());
            ReplayCommand = new Command(() => ReplayCommandExecute());
            SendMessageCommand = new Command(() => SendMessageCommandExecute());
            LoadMoreCommand = new Command(async () => { await LoadMore_click(); });
        }

        private async void SendMessageCommandExecute()
        {
            if (sendEnable)
                return;

            sendEnable = true;

            await SendMessage();

            sendEnable = false;
        }

        private async Task SendMessage()
        {
            try
            {
                if(TypedMessage == null || TypedMessage == "" || string.IsNullOrWhiteSpace(TypedMessage))
                {
                    return;
                }
                else
                {
                    var message = !string.IsNullOrEmpty(TypedMessage.Trim()) ? TypedMessage.Trim() : string.Empty;
                    var values = new Dictionary<object, object>
                    {
                        {"author",userId },
                        {"threadId",threadId},
                        {"plainContent", message},
                        {"createdDate", DateTime.Now.ToUniversalTime() },
                        {"memoToFile",null},
                        {"sendToOfficer",false},
                    };

                    httpClient = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(Config.SAVE_MESSAGE_API, content);
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
                        await ClosePopup();
                        var content1 = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(content1);
                        TypedMessage = null;
                        Settings.TypedMessage = TypedMessage;
                        MessageReplayViewModel thisVm = new MessageReplayViewModel();
                        await FetchThreadUserData();
                        //await SendThreadUsersAsync();
                        //await ShowAlertWithAction("Success", content1, "Ok", "Cancel");
                        //var yesSelected = await DisplayAlert("Simon", content, "Ok", "Cancel"); // the call is awaited
                        //if (yesSelected)  // No compile error, as the result will be bool, since we awaited the Task<bool>
                        //{
                        //    await Navigation.PopToRootAsync();
                        //}
                        //else { return; }
                    }
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                ShowExceptionAlert(ex);
            }
            finally
            {
                await ClosePopup();
            }
        }


        private async Task SendThreadUsersAsync()
        {
            httpClient = new HttpClient();
            if(Settings.MessageThreadUsersData!=null && Settings.MessageThreadUsersData.Count>0)
            {
                var content = new StringContent(JsonConvert.SerializeObject(Settings.MessageThreadUsersData), Encoding.UTF8, "application/json");
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
            }
            else
            {
                await FetchThreadUserData();
            }
        }

        private async void ReplayCommandExecute()
        {
            MessageReplayViewModel replayViewModel = new MessageReplayViewModel();
            replayViewModel.ScreenTitle = Constants.MessageScreenTitle;
            Settings.TypedMessage = TypedMessage;
            await App.Current.MainPage.Navigation.PushAsync(new MessageReplyPage(), false);
        }

        public void PersonDetailsCommandExecute()
        {

        }

        public string plainContent
        {
            get { return _plainContent; }
            set
            {
                _plainContent = value;
                OnPropertyChanged(nameof(plainContent));
            }
        }

        public ObservableCollection<messages> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                OnPropertyChanged(nameof(MessageList));
            }
        }

        public ObservableCollection<messageUsers> ThreadList
        {
            get { return _threadList; }
            set
            {
                _threadList = value;
                OnPropertyChanged(nameof(ThreadList));
            }
        }

        private bool _IsLoadingInfiniteVisible { get; set; } = false;
        public bool IsLoadingInfiniteVisible
        {
            get { return _IsLoadingInfiniteVisible; }
            set
            {
                _IsLoadingInfiniteVisible = value;
                OnPropertyChanged(nameof(IsLoadingInfiniteVisible));
            }
        }

        public async Task FetchThreadUserData()
        {
            _isTeamLoading = true;
            IsLoadingInfiniteEnabled = true;
            App.buttonClick = 0;
            try
            {
                MessageList = new ObservableCollection<messages>();
                IsBusy = true;
                _CurrentPage = 1;
                await LoadData(_CurrentPage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception:>" + ex);
            }
            finally
            {
                IsBusy = false;
                _isTeamLoading = false;
            }
        }

        async Task LoadData(int page)
        {
            var tempOpenData = new ObservableCollection<messages>(MessageList);
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    IsBusy = true;
                    bool IsBookMarkSelect = false;
                    if(Settings.TypedMessage != null)
                    {
                        TypedMessage = Settings.TypedMessage;
                    }

                    var jsonString = await hc.GetStringAsync(Config.MESSAGE_THREAD_API + threadId + "/" + page);
                    
                    if (jsonString != "")
                    {
                        var obj = JsonConvert.DeserializeObject<DealMessageThreadList>(jsonString);
                        if (obj != null)
                        {
                            foreach (var user in obj.messages)
                            {
                                const string HTML_TAG_PATTERN = "<.*?>";
                                if (user.plainContent != null)
                                {
                                    string message = Regex.Replace(user.plainContent, HTML_TAG_PATTERN, string.Empty);
                                    user.plainContent = message;
                                }

                                ThreadList.Clear();
                                string authorIdStr = user.authorId;

                                if (user.messageUsers != null)
                                {
                                    foreach (var thread in user.messageUsers)
                                    {
                                        if(thread.userid_10 == userId)
                                        {
                                            if (thread.followUp == true)
                                            {
                                                IsBookMarkSelect = true;
                                            }
                                            else
                                            {
                                                IsBookMarkSelect = false;
                                            }
                                        }
                                        ThreadList.Add(thread);
                                    }

                                    if (IsBookMarkSelect == true)
                                    {
                                        user.BookMarkImg = "orange_bookmark.png";
                                    }
                                    else
                                    {
                                        user.BookMarkImg = "bookmark.png";
                                    }
                                }

                                if (authorIdStr == null)
                                {
                                    user.HorizontalOption = LayoutOptions.StartAndExpand;
                                    user.IsSenderBookMarkVisible = false;
                                    user.IsSenderProfileVisible = true;
                                    user.IsProfileVisible = false;
                                    user.IsBookMarkVisible = true;
                                }
                                else if (authorIdStr.Equals(userId))
                                {
                                    user.HorizontalOption = LayoutOptions.EndAndExpand;
                                    user.IsSenderBookMarkVisible = true;
                                    user.IsSenderProfileVisible = false;
                                    user.IsProfileVisible = true;
                                    user.IsBookMarkVisible = false;
                                }
                                else
                                {
                                    user.HorizontalOption = LayoutOptions.StartAndExpand;
                                    user.IsSenderBookMarkVisible = false;
                                    user.IsSenderProfileVisible = true;
                                    user.IsProfileVisible = false;
                                    user.IsBookMarkVisible = true;
                                }

                                if (user.plainContent == null)
                                {
                                    user.IsStopVisible = false;
                                    user.HeightRequest = 0;
                                }
                                else if (user.plainContent.Count() < 150)
                                {
                                    user.IsStopVisible = false;
                                    user.HeightRequest = 0;
                                }

                                else if (user.plainContent.Count() > 150)
                                {
                                    user.IsStopVisible = true;
                                    user.moreBtnText = "more";
                                    user.HeightRequest = 35;
                                    user.MaxLines = 3;
                                }
                                TotalRecords = obj.totalRecords;
                                _LastPage = Convert.ToInt32(obj.totalPages);
                                tempOpenData.Add(user);
                            }

                            ObservableCollection<messages> OrderbyIdDesc = new ObservableCollection<messages>(tempOpenData.OrderByDescending(x => x.createdDate.Date));
                            MessageList = new ObservableCollection<messages>(OrderbyIdDesc);
                        }
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task LoadMore_click()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    if (_LastPage == _CurrentPage)
                    {
                        IsLoadingInfinite = false;
                        IsLoadingInfiniteEnabled = false;
                        return;
                    }
                    if (_isTeamLoading) { IsLoadingInfinite = false; return; }
                    //_CurrentPage++;
                    if (App.isFromAddParticipantPage == true)
                    {
                        App.isFromAddParticipantPage = false;
                        _CurrentPage = 1;
                    }
                    else
                    {
                        _CurrentPage++;
                    }

                    await LoadData(_CurrentPage);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ShowExceptionAlert(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
            IsLoadingInfinite = false;
        }

        public async Task PostReadThreadData()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    IsBusy = true;
                    Object userInfo = new { threadId = threadId, userId = userId };

                    var kvalues = new Dictionary<string, object>
                    {
                        {"threadId",threadId},
                        {"userId",userId },
                    };

                    var jObject = JsonConvert.SerializeObject(kvalues);
                    var content = new StringContent(jObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage hs = await hc.PostAsync(Config.MARK_THREADMESSAGE_READ, content);
                    var result = await hs.Content.ReadAsStringAsync();
                    if (hs.IsSuccessStatusCode)
                    { //return Task; 
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public ICommand personIcon_Clicked { get { return new Command<messages>(personIcon_Click); } }
        private async void personIcon_Click(messages list)
        {
            ParticipantListPopup ParticipantPopupview = new ParticipantListPopup();
            ParticipantPopupview.BindingContext = this;
            this.GetParticipantData(list);
            await ShowPopup(ParticipantPopupview);
        }

        public void GetParticipantData(messages messages)
        {
            messageUserList.Clear();
            if (messages.messageUsers.Count != 0)
            {
                IsListDataAvailable = true;
                IsDataNotAvailable = false;
                foreach (var user in messages.messageUsers)
                {
                    messageUserList.Add(user);
                }
            }
            else
            {
                IsDataNotAvailable = true;
                IsListDataAvailable = false;
            }
        }

        public ICommand BookMarkCommand { get { return new Command<messages>(BookMark_click); } }
        private async void BookMark_click(messages list)
        {
            if (list.messageUsers != null)
            {
                if (list.BookMarkImg == "orange_bookmark.png")
                {
                    list.BookMarkImg = "bookmark.png";
                    bookMarks = false;
                }
                else
                {
                    list.BookMarkImg = "orange_bookmark.png";
                    bookMarks = true;
                }

                var values = new Dictionary<object, object>
                {
                    {"name",name },
                    {"userid_10",userId},
                    {"followUp", bookMarks},
                };

                var client = new HttpClient();
                string url = Config.MARK_THREADMESSAGE_BOOKMARK + list.id + "/" + bookMarks + "/" + userId;
                var content1 = new StringContent(JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content1);

                if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.Current.MainPage.DisplayAlert("Data Not Sent!!", string.Format("Response contained status code: {0}", response.StatusCode), "OK");
                    });
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                }

            }
        }

        public ICommand ClosePopup_Command { get { return new Command(ClosePopup_click); } }
        private async void ClosePopup_click()
        {
            await ClosePopup();
        }
    }
}

