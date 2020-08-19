using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Simon.Models
{
    public class DealMainPageList
    {
        public List<DealPage> DealPageList { get; set; }
    }
    public class DealPage
    {
        public string mainTitle { get; set; }
        public string dealType { get; set; }
        public string dealAmount { get; set; }
        public string time { get; set; }
        public string arrow_img { get; set; }

    }
}

