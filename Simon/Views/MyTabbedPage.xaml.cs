using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Simon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTabbedPage : Xamarin.Forms.TabbedPage
    {
        public MyTabbedPage()
        {

            //InitializeComponent();
               
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.Children.Add(new AssignmentPage() { Title = "Assign" });
            this.Children.Add(new MessagesPage() { Title = "Comments" });
           this.Children.Add(new HistoryPage() { Title = "History" });
           // this.Children.Add(new CommentsPageApp() { Title = "History" });

        }
    }
}

