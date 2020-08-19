using System;
using System.Collections.Generic;
using Simon.MenuItems;
using Xamarin.Forms;

namespace Simon.Views
{
    public partial class HomePage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        public HomePage()
        {
            InitializeComponent();

            menuList = new List<MasterPageItem>();

            // Adding menu items to menuList and you can define title ,page and icon
            menuList.Add(new MasterPageItem() { Title = "Home", TargetType = typeof(NotificationPage) });
            menuList.Add(new MasterPageItem() { Title = "Setting",  TargetType = typeof(NotificationPage) });
            menuList.Add(new MasterPageItem() { Title = "Help", TargetType = typeof(NotificationPage) });
            menuList.Add(new MasterPageItem() { Title = "LogOut", TargetType = typeof(NotificationPage) });

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}