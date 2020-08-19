using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Xamarin.Forms;

namespace Simon.SimonRestClient
{
    public class OpenRestClient<T>
    {
        HttpClient _httpClient = new HttpClient();
        IEnumerable<T> enumerableResult;
        IEnumerable<DealMessageList> messageResult;

        public async Task<T> GetMessagesAsync(string userId)
        {
            var json = await _httpClient.GetAsync(Config.MESSAGES_API+userId);
            var assignJson = await json.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(assignJson);
            return result;
        }
        public async Task<T> GetLoginUser()
        {
            var response = await _httpClient.GetAsync(Config.USER_LIST_API);
            var respStr = await response.Content.ReadAsStringAsync();
            if (respStr != "")
                {
                 enumerableResult = JsonConvert.DeserializeObject<IEnumerable<T>>(respStr);

                }
            //Binding listview with server response    
            return (T)enumerableResult;
            }

        public async Task<DealMessageList> GetMessageList(string userId)
        {
            //Check network status   
                try { 
                var response = await _httpClient.GetAsync(Config.MESSAGES_API+userId);
                var assignJson = await response.Content.ReadAsStringAsync();
                if (assignJson != "")
                {
                        //Converting JSON Array Objects into generic list  
                        messageResult = JsonConvert.DeserializeObject<IEnumerable<DealMessageList>>(assignJson);

                   }
                return (Simon.Models.DealMessageList)messageResult;
            }
            catch(System.Exception  e)
            {
                return null;
            }
        }
    }
}

