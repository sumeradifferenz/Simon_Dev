using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Simon.Views
{
    public partial class ThreadMessagePage : ContentPage
    {
        string txtPartyName, txtTopic,userId;
        private MessageThreadViewModel ViewModel = null;
        int threadId;
        DealMessageThreadList messageView = new DealMessageThreadList();
        public ThreadMessagePage()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("PARTYNAME"))
            {
                txtPartyName = Convert.ToString(Application.Current.Properties["PARTYNAME"]);
                lblPartyName.Text = txtPartyName;
            }
            if (Application.Current.Properties.ContainsKey("TOPIC"))
            {
                txtTopic = Convert.ToString(Application.Current.Properties["TOPIC"]);
                lblTopic.Text = txtTopic;
            }
            if (Application.Current.Properties.ContainsKey("THREADID"))
            {
                threadId = (int)Application.Current.Properties["THREADID"];
            }
            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Application.Current.Properties["USERID"]);
            }
            this.BindingContext = this;
            if (NetworkCheck.IsInternet())
            {
                ViewModel = new MessageThreadViewModel();
                ViewModel.PostReadThreadData();
                ViewModel.FetchThreadUserData();
                BindingContext = ViewModel;
            }
            else
            {
                DisplayAlert("Simon", "No network is available.", "OK");
            }
           //SaveThreadMessageRead();
            GetJSON();
        }
        void OnMessageTapped(object sender, EventArgs args)
        {
            var label = sender as Label;
            
            var stackLayout = (sender as Label)?.Parent as StackLayout;
            var user = label.BindingContext as messages;
            if (stackLayout != null)
            {
                //var childrenHeight = stackLayout.Children.Sum(c => c.Height) + stackLayout.Children.Count * stackLayout.Spacing;
                if (label.MaxLines == 3)
                {
                    label.MaxLines=20;
                    user.isLabelVisible = true;
                    //readbtnText.Text = "less";               
                }
                else
                {
                    label.MaxLines = 3;
                }
            }
        }

        //    private void OnTappedMore(object sender, EventArgs e)
        //{
        //    var stackSender = (StackLayout)sender;

        //     //if (ReadButton.Text == "[read more]")
        //    //{
        //    //    //contentLbl.MaxLines = 20;
        //    //    //= NoWrap;
        //    //    ReadButton.Text = "[read less]";
        //    //}
        //    //else
        //    //{
        //    //    //contentLbl.MaxLines = 4;
        //    //    ReadButton.Text = "[read less]";
        //    //}

        //}
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private async void btnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void btnPopUpCancelClicked(object sender, EventArgs e)
        {
           // popUpView.IsVisible = false;
        }
        public async void GetJSON()
        {
            //Check network status   
            if (NetworkCheck.IsInternet())
            {
                var httpClient = new System.Net.Http.HttpClient();

                var response = await httpClient.GetAsync(Config.MESSAGE_THREAD_API + threadId);
                string assignJson = await response.Content.ReadAsStringAsync();
                if (assignJson != "")
                {
                    messageView = JsonConvert.DeserializeObject<DealMessageThreadList>(assignJson);
                }
                //Binding listview with server response  
                //listOfThreadUsers.ItemsSource = messageView.threadUsers;
            }
            else
            {
                await DisplayAlert("JSONParsing", "No network is available.", "OK");
            }
        }

        private void btnReplyClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MessageReplyPage());
        }

        private void readBtnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var user = button.BindingContext as messages;
            if (user.CaretCharacter == "more")
            {
                user.LineBreakMode = LineBreakMode.WordWrap;
                user.IsStopVisible = true;
                //user.CaretCharacter = "less";
                //readbtnText.Text = "less";
                //materialCard.HeightRequest = 300;
            }
            else if (user.CaretCharacter == "less")
            {
                user.CaretCharacter = "more";
                user.IsStopVisible = true;
                user.LineBreakMode = LineBreakMode.TailTruncation;

            }

            //Perform actions on user
        }

        public async void SaveThreadMessageRead()
        {
           var kvalues = new Dictionary<string, object>
            {               
                {"threadId",threadId},
                 {"userId",userId },
            };
           // var jcontent = @"{""threadId"":threadId,""userId"":userId}";
            var client = new HttpClient();      
            var content = new StringContent(JsonConvert.SerializeObject(kvalues), Encoding.UTF8, "application/json");
            var url = Config.MARK_THREADMESSAGE_READ;
            var response = await client.PostAsync(url,content);

            // Debug.WriteLine(content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //await DisplayAlert("Data Not Sent!!", string.Format("Response contained status code: {0}", response.StatusCode), "OK");
                });
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                //var yesSelected = await DisplayAlert("Simon", content, "Ok", "Cancel"); // the call is awaited
                //if (yesSelected)  // No compile error, as the result will be bool, since we awaited the Task<bool>
                //{
                //    await Navigation.PopToRootAsync();
                //}
                //else { return; }
            }

        }

    }
}
