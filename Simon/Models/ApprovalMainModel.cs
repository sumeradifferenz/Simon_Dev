using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Simon.Models
{
    public partial class ApprovalMainModel
    {

        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("reqname_10")]
        public string reqname_10 { get; set; }
        [JsonProperty("productdesc_10")]
        public string productdesc_10 { get; set; }
        [JsonProperty("amount_06")]
        public double amount_06 { get; set; }
        [JsonProperty("partyname_10")]
        public string partyname_10 { get; set; }
        [JsonProperty("requirementid_05")]
        public int requirementid_05 { get; set; }
        [JsonProperty("approvaltype")]
        public string approvaltype { get; set; }
        [JsonProperty("stageentrydatetime_10")]
        public string stageentrydatetime_10 { get; set; }
        [JsonProperty("dealid_05")]
        public string dealid_05 { get; set; }
        public DateTime ApprovalDate { get; set; }
    }

    public partial class ApprovalMainModel
    {
        public static ObservableCollection<ApprovalMainModel> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<ApprovalMainModel>>(json);
    }
}

