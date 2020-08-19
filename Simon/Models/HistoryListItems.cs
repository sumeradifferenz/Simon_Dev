using System;
using System.Collections.Generic;
using Xamarin.Forms;

    namespace Simon.Models
    {
        public class HistoryListItems
        {
            public List<history> historyList { get; set; }
        }
        public class history
       {
        public string titleImg { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string arrow_img { get; set; }

        }
           
     }


