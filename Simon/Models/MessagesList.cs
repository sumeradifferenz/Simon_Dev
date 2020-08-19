using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Simon.Models
{
    public class MessagesList
    {
        public List<notificationMsg> messages { get; set; }
        public List<threadUsersList> threadUsers { get; set; }

    }
    public class notificationMsg
    {
        public string id { get; set; }
        public string mainTitleMsg { get; set; }
        public string mainSubTitleMsg { get; set; }
        public string mainTitleColor { get; set; }
        public string mainSubTitleColor { get; set; }
        public string msgTitle { get; set; }
        public string msgSubTitle { get; set; }
    }
    public class threadUsersList
    {
        public string id { get; set; }
        public string msgTitle { get; set; }
        public string msgSubTitle { get; set; }
    }
}
