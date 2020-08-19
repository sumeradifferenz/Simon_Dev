using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Simon.Models
{
    public class UserNameList
    {
        public List<UserData> UserLists { get; set; }
    }
    public class UserData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string companyId { get; set; }
        public string role { get; set; }
        public string primaryRole { get; set; }
    }
}

