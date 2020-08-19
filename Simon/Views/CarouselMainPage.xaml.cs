using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Plugin.Settings;
using Simon.Helpers;
using Simon.ServiceHandler;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Views
{
    public partial class CarouselMainPage : CarouselPage
    {
        private BaseViewModel vm = null;
        int id { get; set; }
        public CarouselMainPage(int _id)
        {
            InitializeComponent();

            id = _id;

            CrossSettings.Current.Remove(Settings.ApprovePageSelectedTabKey);

            carouselmainpage.Children.Add(new LandingPage());
            carouselmainpage.Children.Add(new DealsPage());
            carouselmainpage.Children.Add(new MessagesPage());
            carouselmainpage.Children.Add(new AssentMainPage());

            var index = Children.IndexOf(CurrentPage);
            var pageCount = Children.Count;

            try
            {
                //switch (id)
                //{
                //    case 0:
                //        CurrentPage = Children[id];
                //        id = 0;
                //        break;
                //    case 1:
                //        CurrentPage = Children[id];
                //        id = 0;
                //        break;
                //    case 2:
                //        CurrentPage = Children[id];
                //        id = 0;
                //        break;
                //    case 3:
                //        CurrentPage = Children[id];
                //        id = 0;
                //        break;
                //}
                CurrentPage = Children[id];
                id = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnCurrentPageChanged()
        {
            int index = Children.IndexOf(CurrentPage);
            if (id != 0)
            {
                index = id;
            }

            vm = new BaseViewModel();
            this.BindingContext = vm;

            foreach (var item in vm.FooterItems)
            {
                if (item.Id == index)
                {
                    SessionService.SelectedFooterItem = item;

                    SessionService.BaseFooterItems.All((arg) =>
                    {
                        if (arg.Id == item.Id)
                        {
                            arg.IsSelected = true;
                        }
                        else
                        {
                            arg.IsSelected = false;
                        }
                        return true;
                    });
                }
            }
        }
    }
}
