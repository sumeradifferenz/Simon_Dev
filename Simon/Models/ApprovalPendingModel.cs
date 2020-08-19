using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Simon.Models
{
    public partial class ApprovalPendingModel 
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("shortMobileName")]
        public string shortMobileName { get; set; }

        [JsonProperty("functionId")]
        public string functionId { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("notifyDefinition")]
        public string notifyDefinition { get; set; }

        [JsonProperty("processStageFunctionId")]
        public string processStageFunctionId { get; set; }
    }

    public partial class ApprovalPendingModel
    {
        public static ObservableCollection<ApprovalPendingModel> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<ApprovalPendingModel>>(json);
    }
}

