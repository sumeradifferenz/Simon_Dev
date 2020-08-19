using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Simon.Models
{
    public class DealsMainModel
    {
        public string dealId { get; set; }
        public string partyName { get; set; }
        public string product { get; set; }
        public double amount { get; set; }
        public string status { get; set; }
        public string primaryuserid_10 { get; set; }
        Nullable<DateTime> dt = DateTime.Now.Date;
        public Nullable<DateTime> estimatedClosingDate { get; set; }
        public Nullable<DateTime> decisionDueDate { get; set; }
        public int totalRecords { get; set; }
        public double totalPages { get; set; }

    }

    public class Stage
    {
        public string stageid_10 { get; set; }
        public string stagelabel_10 { get; set; }
    }

    //public partial class DealsMainModel
    //{
    //    public static ObservableCollection<DealsMainModel> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<DealsMainModel>>(json);
    //}
}

