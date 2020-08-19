
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;  
using Xamarin.Forms;
using Simon.Models;
using Simon.Helpers;
using Newtonsoft.Json;

namespace Simon.Views
{
    public partial class HistoryPage : ContentPage
    {
        public List <HistoryListItems> tempdata;
        HistoryListItems ObjHistoryList = new HistoryListItems();

        public HistoryPage()
        {
            InitializeComponent();
            historyData();
            listOfHistory.ItemsSource = ObjHistoryList.historyList;        
        }
    public async void historyData()
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
            string assignJson = "{\"historyList\": [{\"title\": \"Trevor Johnson\",\"date\": \"04/30/2019\",\"time\":\"10.00am\"},{\"title\": \"Trevor Johnson\",\"date\": \"04/30/2019\",\"time\":\"10.00am\"},{\"title\": \"Trevor Johnson\",\"date\": \"04/30/2019\",\"time\":\"10.00am\"},{\"title\": \"Trevor Johnson\",\"date\": \"04/30/2019\",\"time\":\"10.00am\"}]}";

            //*****************************************************************

            //string assignJson = await response.Content.ReadAsStringAsync();
            if (assignJson != "")
            {
                    //Converting JSON Array Objects into generic list  
                    ObjHistoryList = JsonConvert.DeserializeObject<HistoryListItems>(assignJson);
            }
                //Binding listview with server response    
                listOfHistory.ItemsSource = ObjHistoryList.historyList;
        }
        else
        {
            await DisplayAlert("JSONParsing", "No network is available.", "Ok");
        }
        //Hide loader after server response    
        //ProgressLoader.IsVisible = false;    
}
}
}