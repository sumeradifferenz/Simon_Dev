using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Simon.Models
{
    public class AssignListItems 
    {

        public List<Assign> Assignments { get; set; }
    }
    public class Assign
    {
        public string id { get; set; }
        public string titleName { get; set; }
        public string subTitleName { get; set; }
        public string imgsource { get; set; }
    }
}


