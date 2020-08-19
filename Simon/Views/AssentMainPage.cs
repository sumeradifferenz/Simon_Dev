using System;

using Xamarin.Forms;

namespace Simon.Views
{
    public class AssentMainPage : ContentPage
    {
        public AssentMainPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

