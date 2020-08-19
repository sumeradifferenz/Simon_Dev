using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Xamarin.Forms;

namespace Simon.Views
{
    public partial class NotificationAlertPage : ContentPage
    {
        //private ObservableCollection<MessagesList> _followerItems;
        
        public List<MessagesList> tempdata;
        //MessagesList msgList = new MessagesList();
        MessagesList ObjAssignList = new MessagesList();
        public NotificationAlertPage()
        {
            InitializeComponent();
            GetJSON();
            list.ItemsSource = ObjAssignList.messages;

        }
        private async void tab1_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyTabbedPage());
        }
        private async void tab2_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssentMainPage());
        }

        public async void GetJSON()
        {
            //Check network status   
            if (NetworkCheck.IsInternet())
            {

                //var httpClient = new System.Net.Http.HttpClient();
                //var response = await httpClient.GetAsync("REPLACE YOUR JSON URL");
                //****************************************************************
                // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // httpClient.DefaultRequestHeaders.Add("APP_VERSION", "1.0.0");
                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_authorization_token_string");
                string assignJson = "{\"messages\":[{\"mainTitleMsg\":\"New Message\",\"mainSubTitleMsg\": \"Hey Trevor, we receive a title work \"},{\"mainTitleMsg\":\"Old Message\",\"mainSubTitleMsg\": \"Hey Trevor, we receive a title work \"},{\"mainTitleMsg\":\"Latest Message\",\"mainSubTitleMsg\": \"Hey Trevor, we receive a title work \"},{\"mainTitleMsg\":\"Todays Message\",\"mainSubTitleMsg\": \"Hey Trevor, we receive a title work \"}]}";

                //*****************************************************************

                //string assignJson = await response.Content.ReadAsStringAsync();

                if (assignJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ObjAssignList = JsonConvert.DeserializeObject<MessagesList>(assignJson);
                }
                //Binding listview with server response    
                list.ItemsSource = ObjAssignList.messages;
            }
            else
            {
                await DisplayAlert("JSONParsing", "No network is available.", "Ok");
            }
            //Hide loader after server response    
            //ProgressLoader.IsVisible = false;
        }

        private void replyBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MessageReplyPage());
        }

        private void closeBtnClicked(object sender, EventArgs e)
        {
            popupMsgThread.IsVisible = false;
            // activityIndicator.IsRunning = true;
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //thats all you need to make a search  

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list.ItemsSource = ObjAssignList.messages;
            }

            else
            {

                // list.ItemsSource = tempdata.Where(x => x.mainTitleMsg.StartsWith(e.NewTextValue));
                //var city = (MessagesList)(sender);
               
                var lowerKeyword = e.NewTextValue.ToLower();
                //list.ItemsSource = msgList.messages
                    //.Where(r => r.mainTitleMsg.ToLower().Contains(lowerKeyword)).ToList();



               list.ItemsSource = ObjAssignList.messages.Where(x => x.mainTitleMsg.StartsWith(e.NewTextValue));

            }
        }
    }
}