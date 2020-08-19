using Simon.MenuItems;
using Simon.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Simon
{
    public partial class DashboardPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        string userNametxt, primaryRoletxt;
        public DashboardPage()
        {
            InitializeComponent();
            NavigationPage navPage = new NavigationPage
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.Black
            };
            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(LandingPage)));
            if (Application.Current.Properties.ContainsKey("NAME"))
            {
                userNametxt = Convert.ToString(Application.Current.Properties["NAME"]);
            }
            if (Application.Current.Properties.ContainsKey("PRIMARYROLE"))
            {
                primaryRoletxt = Convert.ToString(Application.Current.Properties["PRIMARYROLE"]);
            }
            txtName.Text = userNametxt;
            txtPrimaryRole.Text = primaryRoletxt;
        }
       
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }

    }
}
