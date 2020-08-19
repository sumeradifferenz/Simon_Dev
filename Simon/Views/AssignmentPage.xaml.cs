
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;

using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;  
using Xamarin.Forms;
namespace Simon.Views
{
    public partial class AssignmentPage : ContentPage
    {
        public List<AssignListItems> tempdata;
        AssignListItems ObjAssignList = new AssignListItems();
        public AssignmentPage()
        {
            InitializeComponent();
            GetJSON();
            list.ItemsSource = ObjAssignList.Assignments;
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
                string assignJson = "{\"Assignments\": [{\"titleName\": \"Trevor Johnson\",\"subTitleName\": \"Executive Vice President\"},{\"titleName\": \"John Trevorson\",\"subTitleName\": \"Executive Vice President\"},{\"titleName\": \"Sidney Sheldon\",\"subTitleName\": \"Executive Vice President\"}]}";

                //*****************************************************************

                //string assignJson = await response.Content.ReadAsStringAsync();
                if (assignJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ObjAssignList = JsonConvert.DeserializeObject<AssignListItems>(assignJson);
                }
                //Binding listview with server response    
                list.ItemsSource = ObjAssignList.Assignments;
            }
            else
            {
                await DisplayAlert("JSONParsing", "No network is available.", "Ok");
            }
            //Hide loader after server response    
            //ProgressLoader.IsVisible = false;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //thats all you need to make a search  

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list.ItemsSource = ObjAssignList.Assignments;
            }

            else
            {
                // list.ItemsSource = tempdata.Where(x => x.mainTitleMsg.StartsWith(e.NewTextValue));
                //var city = (MessagesList)(sender);
                var lowerKeyword = e.NewTextValue.ToLower();
                //list.ItemsSource = msgList.messages
                //.Where(r => r.mainTitleMsg.ToLower().Contains(lowerKeyword)).ToList();
                list.ItemsSource = ObjAssignList.Assignments.Where(x => x.titleName.StartsWith(e.NewTextValue));

            }
        }

    }
}