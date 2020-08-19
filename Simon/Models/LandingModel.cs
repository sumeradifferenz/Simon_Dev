using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Simon.Models
{
    public partial class LandingModel
    {
        internal string Amount1;

        public string dealId { get; set; }
        public string partyName { get; set; }
        public Nullable<DateTime> estimatedClosingDate { get; set; }
        public Nullable<DateTime> decisionDueDate { get; set; }
        public double amount { get; set; }
        public string Date { get; set; }
        public string Borrower { get; set; }
        public string Amount { get; set; }
    }
    public partial class LandingModel
    {
        public static ObservableCollection<LandingModel> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<LandingModel>>(json);
    }

}

