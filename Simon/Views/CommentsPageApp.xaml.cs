using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Xamarin.Forms;

namespace Simon.Views
{
    public partial class CommentsPageApp : ContentPage
    {
     //   public List<CommentsListItems> tempdata;
     //   string _dataItem;
     //   CommentsListItems ObjCommentList = new CommentsListItems();
     //   public CommentsPageApp(string dataItem)
     //   {
     //       InitializeComponent();
     //       GetJSON();
     //       _dataItem = dataItem.ToString();

     //      listOfComments.ItemsSource = ObjCommentList.comments;
     //   }
     //public async void GetJSON()
        //{
        //    //Check network status   
        //    if (NetworkCheck.IsInternet())
        //    {

        //        //var httpClient = new System.Net.Http.HttpClient();
        //        //var response = await httpClient.GetAsync("REPLACE YOUR JSON URL");
        //        //****************************************************************
        //       // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //       // httpClient.DefaultRequestHeaders.Add("APP_VERSION", "1.0.0");
        //       // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_authorization_token_string");
        //       string assignJson = "{\"comments\": [{\"commentsTxt\": \"I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this.I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this\",\"mitigatingFactorTxt\": \"I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this,I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this\"}]}";

        //        //*****************************************************************

        //        //string assignJson = await response.Content.ReadAsStringAsync();
        //        if (assignJson != "")
        //        {
        //            //Converting JSON Array Objects into generic list  
        //            ObjCommentList = JsonConvert.DeserializeObject<CommentsListItems>(assignJson);
        //        }
        //        //Binding listview with server response    
        //        listOfComments.ItemsSource = ObjCommentList.comments;
        //    }
        //    else
        //    {
        //        await DisplayAlert("JSONParsing", "No network is available.", "Ok");
        //    }
        //    //Hide loader after server response    
        //    //ProgressLoader.IsVisible = false;
        //}
        //public async void PostJSON()
        //{
        //    //Check network status   
        //    if (NetworkCheck.IsInternet())
        //    {

        //        //var httpClient = new System.Net.Http.HttpClient();
        //        //var response = await httpClient.PostAsync("REPLACE YOUR JSON URL");
        //        //****************************************************************
        //        // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        // httpClient.DefaultRequestHeaders.Add("APP_VERSION", "1.0.0");
        //        // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_authorization_token_string");
        //        string assignJson = "{\"comments\": [{\"commentsTxt\": \"I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this.I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this\",\"approvalConditionTxt\": \"I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this,I am Requesting for the Loan Approval based on this criteria,Requesting\n for the Loan Approval based on this criteri m Requesting for the Loan Approval based on this\"}]}";

        //        //*****************************************************************

        //        //string assignJson = await response.Content.ReadAsStringAsync();
        //        if (assignJson != "")
        //        {
        //            //Converting JSON Array Objects into generic list  
        //            ObjCommentList = JsonConvert.DeserializeObject<CommentsListItems>(assignJson);
        //        }
        //        //Binding listview with server response    
        //        listOfComments.ItemsSource = ObjCommentList.comments;
        //    }
        //    else
        //    {
        //        await DisplayAlert("JSONParsing", "No network is available.", "Ok");
        //    }
        //    //Hide loader after server response    
        //    //ProgressLoader.IsVisible = false;
        //}
        //private void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    ObservableCollection<CommentsListItems> l = new ObservableCollection<CommentsListItems>();           
        //   // System.Diagnostics.Debug.WriteLine("idx = " + idx.ToString());
        //    CommentsListItems ObjCommentList = e.SelectedItem as CommentsListItems;          
        //     // do something with your item :)
        //    }

        //     private void onEditClicked(object sender, EventArgs e)
        //{
        //    //Navigation.PushAsync(new MessageThreadPage());
        //   // entryComment.IsReadOnly = false;

        //}

        //public async void onApproveBtnClicked(object sender, EventArgs e)
        //{
        //    //Navigation.PushAsync(new MessageThreadPage());
        //    // entryComment.IsReadOnly = false;
        //    var result = await DisplayAlert("SIMON", "Data Saved Successfully", "Ok", "Cancel");
        //    if (result)
        //    {
        //        // Navigation.PushAsync(new DashboardPage());
        //        return;
        //    }
        //}
    }
    }
