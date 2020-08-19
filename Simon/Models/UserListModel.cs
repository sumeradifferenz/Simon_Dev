using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Models
{
    public partial class UserListModel : BaseViewModel
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("companyId")]
        public string companyId { get; set; }
        [JsonProperty("role")]
        public string role { get; set; }
        [JsonProperty("primaryRole")]
        public string primaryRole { get; set; }
        [JsonProperty("roles")]
        public List<Role> roles { get; set; }
        [JsonProperty("firstName")]
        public string firstName { get; set; }

        [JsonIgnore]
        private string _Radiobtnimg = "radio_unselect.png";
        public string Radiobtnimg
        {
            get
            {
                return _Radiobtnimg;
            }
            set
            {
                SetProperty(ref _Radiobtnimg, value);
            }
        }
        [JsonIgnore]
        private Style _NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
        public Style NamelblStyle
        {
            get
            {
                return _NamelblStyle;
            }
            set
            {
                SetProperty(ref _NamelblStyle, value);
            }
        }
    }

    public class Role
    {
        public string roleName { get; set; }
        public bool isPrimary { get; set; }
    }

    public partial class UserListModel
    {
        public static ObservableCollection<UserListModel> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<UserListModel>>(json);
    }
}

