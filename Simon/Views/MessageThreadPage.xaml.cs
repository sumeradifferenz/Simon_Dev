using System;
using System.Linq;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Simon.Views
{
    public partial class MessageThreadPage : ContentPage
    {      
        string txtPartyName,txtTopic;
        private MessageThreadViewModel ViewModel = null;
        int threadId;
        DealMessageThreadList messageView = new DealMessageThreadList();       
        public MessageThreadPage()
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
            if (NetworkCheck.IsInternet())
            {
                ViewModel = new MessageThreadViewModel();
                 ViewModel.FetchThreadUserData();
                BindingContext = ViewModel;
            }
            else
            {
                DisplayAlert("Simon", "No network is available.", "Ok");
            }

            GetJSON();
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
       
        private void btnPopUpCancelClicked(object sender,EventArgs e)
        {
            popUpView.IsVisible = false;
        }
        public async void GetJSON()
        {
            //Check network status   
            if (NetworkCheck.IsInternet())
            {
              var httpClient = new System.Net.Http.HttpClient();

                var response = await httpClient.GetAsync(Config.MESSAGE_THREAD_API+threadId);
                string assignJson = await response.Content.ReadAsStringAsync();
                if (assignJson != "")
                {
                 messageView = JsonConvert.DeserializeObject<DealMessageThreadList>(assignJson);
                }
                //Binding listview with server response  
               listOfThreadUsers.ItemsSource = messageView.threadUsers;
            }
           else
           {
                await DisplayAlert("JSONParsing", "No network is available.", "Ok");
            }
       }

        private async void personIcon_Clicked(object sender, EventArgs e)
        {              
            var searchList = messageView.threadUsers.Select(x => x.name).ToList();
            string[] sampleArray = searchList.ToArray();
            if (sampleArray.Count() >= 1)
            {
                var action = await DisplayActionSheet("Participants", null, "Close", sampleArray);
                if (action == "Close")
                { return; }
            }
            else
            {
                await DisplayAlert("Simon", "Participants Not available.", "Ok");
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
                user.CaretCharacter = "less";
                materialCard.HeightRequest = 300;
            }
            else if (user.CaretCharacter == "less")
            {
                user.CaretCharacter = "more";
                user.IsStopVisible = true;
                user.LineBreakMode = LineBreakMode.TailTruncation;             

            }

            //Perform actions on user
        }

        //private async void onSelectedItem(object sender, SelectedItemChangedEventArgs e)
        //{
        //    DealMessageList item = ((ListView)sender).SelectedItem as DealMessageList;
        //    int threadIdNo = item.threadId;
        //    string strPartyName = item.partyName;
        //    string strTopic = item.topic;
        //    Application.Current.Properties["THREADID"] = threadIdNo;
        //    Application.Current.Properties["PARTYNAME"] = strPartyName;
        //    Application.Current.Properties["TOPIC"] = strTopic;
        //    await Application.Current.SavePropertiesAsync();
        //    await Navigation.PushAsync(new MessageThreadPage());
        //    //searchView.IsVisible = false;
        //    item = null;
        //}

    }
}
