using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DLToolkit.Forms.Controls;
using Newtonsoft.Json;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Models
{
    public partial class DealMessageList : BaseViewModel
    { 
        [JsonProperty("threadId")]
        public int threadId { get; set; }
        [JsonProperty("dealId")]
        public int dealId { get; set; }
        [JsonProperty("topic")]
        public string topic { get; set; }
        [JsonProperty("lastMessage")]
        public string lastMessage { get; set; }
        [JsonProperty("lastPostDate")]
        public Nullable<DateTime> lastPostDate { get; set; }
        [JsonProperty("lastPostBy")]
        public string lastPostBy { get; set; }
        [JsonProperty("hasBeenRead")]
        public bool hasBeenRead { get; set; }
        [JsonProperty("partyName")]
        public string partyName { get; set; }
        [JsonProperty("followupExist")]
        public bool followupExist { get; set; }
        [JsonProperty("totalRecords")]
        public int totalRecords { get; set; }
        [JsonProperty("totalPages")]
        public double totalPages { get; set; }

       
        

        public bool IsRedDotVisible { get; set; }
        public bool IsBookMarkVisible { get; set; }
        public Style LastMsgStyle { get; set; }

        public ImageSource _Switchimg { get; set; } = "orange_bookmark.png";
        public ImageSource Switchimg
        {
            get { return _Switchimg; }
            set
            {
                _Switchimg = value;
                OnPropertyChanged("Switchimg");
            }
        }
    }
    public partial class DealMessageList
    {
        public static ObservableCollection<DealMessageList> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<DealMessageList>>(json);
    }

}

