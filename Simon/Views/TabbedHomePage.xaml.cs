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
    public partial class TabbedHomePage : Xamarin.Forms.TabbedPage
    {
        public TabbedHomePage()
        {
            //InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
           
            this.Children.Add(new MessagesPage() { Title = "Portfolio" });
            this.Children.Add(new DealsPage() { Title = "Deals" });

            this.Children.Add(new HistoryPage() { Title = "Messages" });
            this.Children.Add(new HistoryPage() { Title = "Alerts" });
            this.Children.Add(new HistoryPage() { Title = "Approve" });
            this.BarBackgroundColor = Color.FromHex("#0D4263");
            this.BarTextColor = Color.White;
        }
    }
}
