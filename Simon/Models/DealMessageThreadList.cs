using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Models
{
    public partial class messageUsers : BaseViewModel
    {
        public string name { get; set; }
        public string userid_10 { get; set; }
        public bool? followUp { get; set; }
    }

    public partial class messages : BaseViewModel
    {
        public int id { get; set; }
        public string plainContent { get; set; }
        public string threadId { get; set; }
        public string authorId { get; set; }
        public string author { get; set; }
        public DateTime createdDate { get; set; }
        public Xamarin.Forms.LayoutOptions HorizontalOption { get; set; }
        public int MaxLines { get; set; }
        public bool IsStopVisible { get; set; }
        public Xamarin.Forms.LineBreakMode LineBreakMode { get; set; }
        public string CaretCharacter { get; set; }
        public double HeightRequest { get; set; }
        public int Height { get; set; }
        public bool isLabelVisible { get; set; }
        public string moreBtnText { get; set; }
        public List<messageUsers> messageUsers { get; set; }

        public bool IsBookMark { get; set; }
        public bool IsBookMarkVisible { get; set; } = false;
        public bool IsSenderBookMarkVisible { get; set; } = false;
        public bool IsSenderProfileVisible { get; set; } = false;
        public bool IsProfileVisible { get; set; } = false;

        public string _BookMarkImg { get; set; } = "bookmark.png";
        public string BookMarkImg
        {
            get { return _BookMarkImg; }
            set
            {
                _BookMarkImg = value;
                OnPropertyChanged("BookMarkImg");
            }
        }
    }

    public class DealMessageThreadList : BaseViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public int dealId { get; set; }
        public List<messages> messages { get; set; }
        public int totalRecords { get; set; }
        public double totalPages { get; set; }
    }

    //public partial class messages
    //{
    //    public static ObservableCollection<messages> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<messages>>(json);
    //}
}

