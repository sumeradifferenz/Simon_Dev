using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Simon.Models
{
    public partial class MessageThread
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("companyId")]
        public object companyId { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("role")]
        public object role { get; set; }

        [JsonProperty("primaryRole")]
        public object primaryRole { get; set; }

        [JsonProperty("roles")]
        public object roles { get; set; }

        [JsonProperty("firstName")]
        public object firstName { get; set; }
    }
    public partial class MessageThread
    {
        public static ObservableCollection<MessageThread> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<MessageThread>>(json);
    }
}
