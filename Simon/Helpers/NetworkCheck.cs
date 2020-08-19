using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Simon.Helpers
{
        public class NetworkCheck
        {
            public static bool IsInternet()
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    return true;
                }
                else
                {
                    // write your code if there is no Internet available      
                    return false;
                }
            }
        }
    }  