using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.SimonRestClient;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.ServiceHandler
{
    public class RestServices
    {
       public string userId;
        public RestServices()
        {
            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Application.Current.Properties["USERID"]);
            }
        }
    // Get new data rows
    public  async Task GetAllNewsAsync(Action<IEnumerable<LandingModel>> action)
        {
           
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(Config.CLOSING_API + userId);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<IEnumerable<LandingModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }

        }
    }
}
